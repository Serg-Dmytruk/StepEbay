using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StepEbay.Common.Constans;
using StepEbay.Common.Models.ProductInfo;
using StepEbay.Data;
using StepEbay.Worker.ClientHubs;

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
            await Task.WhenAll(Task.Run(() => CloseBuying(), stoppingToken));
        }

        private async Task InvopkeBetIvent(List<ProductInfo> users, List<ProductInfo> owners)
        {
            if (users.Any())
                await _betHubClient.SendBetInfo(users);
            if (owners.Any())
                await _betHubClient.SendOwnerInfo(owners);
        }

        private async Task CloseBuying()
        {
            while (true)
            {
                using var scope = _services.CreateScope();
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await using var dbTransaction = await db.Database.BeginTransactionAsync();

                    var closeId = (await db.PurchaseStates.SingleAsync(x => x.State == PurchaseStatesConstant.CLOSE)).Id;

                    try
                    {
                        var purchases = await db.Purchases.Include(x => x.Product)
                            .Where(x => x.Product.IsActive
                            && x.Product.DateClose <= DateTime.UtcNow.AddMinutes(-1)
                            && x.PurchaseStateId != closeId).ToListAsync();

                        purchases.ForEach(x => { x.Product.IsActive = false; x.PurchaseStateId = closeId; });

                        db.Purchases.UpdateRange(purchases);

                        await db.SaveChangesAsync();
                        await dbTransaction.CommitAsync();

                        var users = purchases.Select(x => new ProductInfo { UserId = x.UserId, ProductId = x.PoductId }).ToList();
                        var owners = purchases.Select(x => new ProductInfo { UserId = x.Product.OwnerId, ProductId = x.PoductId }).ToList();

                        await InvopkeBetIvent(users, owners);

                        await Task.Delay(500);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                        await dbTransaction.RollbackAsync();
                        await Task.Delay(1000);
                    }
                }
            }
        }

        private async Task PriceChange()
        {
            while (true)
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    await Task.Delay(1000);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                    await Task.Delay(5000);
                }
            }
        }
    }
}
