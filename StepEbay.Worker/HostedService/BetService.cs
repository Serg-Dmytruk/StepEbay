using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StepEbay.Common.Constans;
using StepEbay.Common.Models.ProductInfo;
using StepEbay.Data;
using StepEbay.Data.Models.Bets;
using StepEbay.Data.Models.Products;
using StepEbay.Worker.ClientHubs;
using StepEbay.Worker.Comparers;

namespace StepEbay.Worker.HostedService
{
    public class BetService : BackgroundService
    {
        private readonly BetHubClient _betHubClient;
        private readonly IServiceProvider _services;
        private readonly ILogger<BetService> _logger;

        public BetService(BetHubClient betHubClient, IServiceProvider services, ILogger<BetService> logger)
        {
            _betHubClient = betHubClient;
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _betHubClient.Start();
            await StartScanningBets(stoppingToken);
        }

        private async Task StartScanningBets(CancellationToken stoppingToken)
        {
            //запускаємо асинхронно відслідковування закриття товарів, ставок, та зміни цін
            await Task.WhenAll(Task.Run(() => CloseBets(), stoppingToken),
                Task.Run(() => DeactivateProducts(), stoppingToken),
                Task.Run(() => PriceChange(), stoppingToken));
        }

        private async Task InvopkeBetIvent(List<ProductInfo> users = null,
            List<ProductInfo> ownersBuy = null,
            List<ProductInfo> ownersClose = null,
            List<ChangedPrice> changedPrices = null)
        {
            if (users is not null && users.Any())
                await _betHubClient.SendBetInfo(users);
            if (ownersBuy is not null && ownersBuy.Any())
                await _betHubClient.SendOwnerInfo(ownersBuy);
            if (ownersClose is not null && ownersClose.Any())
                await _betHubClient.SendOwnerClose(ownersClose);
            if (changedPrices is not null && changedPrices.Any())
                await _betHubClient.SendPriceChanged(changedPrices);
                
        }

        private async Task CloseBets()
        {
            while (true)
            {
                using var scope = _services.CreateScope();
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await using var dbTransaction = await db.Database.BeginTransactionAsync();

                    var closeId = (await db.PurchaseStates.SingleAsync(x => x.State == PurchaseStatesConstant.CLOSE)).Id;

                    List<Purchase> purchasesInvoke = new();
                    try
                    {
                        //Вибераємо товари (тип покупки аукціон) де термін дії підходить кінця та змінюємо стан на закритий
                        purchasesInvoke = await db.Purchases.Include(x => x.Product)
                           .Where(x => x.Product.IsActive
                           && x.Product.PurchaseType.Type == PurchaseTypesConstant.AUCTION
                           && x.Product.DateClose <= DateTime.UtcNow.AddMinutes(-1)
                           && x.PurchaseStateId != closeId).ToListAsync();

                        purchasesInvoke.ForEach(x => { x.Product.IsActive = false; x.PurchaseStateId = closeId; });

                        db.Purchases.UpdateRange(purchasesInvoke);

                        await db.SaveChangesAsync();
                        await dbTransaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                        await dbTransaction.RollbackAsync();
                        await Task.Delay(10000);
                    }

                    var users = purchasesInvoke.Select(x => new ProductInfo { UserId = x.UserId, ProductId = x.PoductId }).ToList();
                    var owners = purchasesInvoke.Select(x => new ProductInfo { UserId = x.Product.OwnerId, ProductId = x.PoductId }).ToList();

                    await InvopkeBetIvent(users, owners, null);
                    await Task.Delay(10000);
                }
            }
        }

        private async Task DeactivateProducts()
        {
            while (true)
            {
                using var scope = _services.CreateScope();
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await using var dbTransaction = await db.Database.BeginTransactionAsync();

                    List<Product> productsInvoke = new();
                    try
                    {
                        //Вибераємо товари (тип покупки звичайний) де термін дії підходить кінця та змінюємо стан на закритий
                         productsInvoke = await db.Products
                            .Where(x => x.IsActive
                            && x.DateClose <= DateTime.UtcNow.AddMinutes(-1)
                            && !db.Purchases.Any(p => p.PoductId == x.Id)).ToListAsync();

                        productsInvoke.ForEach(x => { x.IsActive = false; });

                        db.Products.UpdateRange(productsInvoke);

                        await db.SaveChangesAsync();
                        await dbTransaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                        await dbTransaction.RollbackAsync();
                        await Task.Delay(10000);
                    }

                    var owners = productsInvoke.Select(x => new ProductInfo { UserId = x.OwnerId, ProductId = x.Id }).ToList();

                    await InvopkeBetIvent(null, null, owners);
                    await Task.Delay(10000);                
                }
            }
        }

        private async Task PriceChange()
        {
            List<ChangedPrice> changedPrices = new();

            while (true)
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    //Вибераємо все шо в покупаках аукціон та макс ціну де стан ще не закритий
                    var auctiones = await db.Purchases.Include(p => p.Product).ThenInclude(p => p.PurchaseType)
                        .Include(p => p.PurchaseState)
                        .Where(p => p.Product.PurchaseType.Type == PurchaseTypesConstant.AUCTION
                        && p.Product.IsActive
                        && p.PurchaseState.State == PurchaseStatesConstant.OPEN)
                        .GroupBy(p => p.PoductId)
                        .Select(cp => new ChangedPrice 
                        {
                            ProductId = cp.Key,
                            Price = cp.Max(x => x.PurchasePrice),
                        }).ToListAsync();

                    //Якщо в памяті ще нічого нема то просто присвоємо перевіремо в наступній ітерації
                    if(!changedPrices.Any())
                        changedPrices = auctiones;
                    else
                    {
                        //Очищаємо з памяті перед порівнняам те чого нема у вибірці з бд так як аукціон міг бути закритим
                        var closedBets = changedPrices
                            .Where(x => !auctiones.Select(a => a.ProductId).Contains(x.ProductId)).ToList();

                        changedPrices.RemoveAll(x => closedBets.Select(c => c.ProductId).Contains(x.ProductId));

                        //Вибераємо всі аукціони де були зміни з останьою вибіркою з бази
                        var changed = auctiones.Intersect(changedPrices, new ChangedPriceComparer()).ToList();
                        changedPrices = auctiones;

                        await InvopkeBetIvent(null, null, null, changed);
                    }

                    await Task.Delay(1000);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    await Task.Delay(1000);
                }
            }
        }
    }
}

