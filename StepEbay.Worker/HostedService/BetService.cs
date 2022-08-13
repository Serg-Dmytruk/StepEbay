using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StepEbay.Common.Constans;
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
            await StartScanningBets(stoppingToken);
        }

        private async Task StartScanningBets(CancellationToken stoppingToken)
        {
            await Task.WhenAll(Task.Run(() => CloseBets(), stoppingToken));
        }

        private async Task InvopkeBetIvent(List<int> users)
        {
            await _betHubClient.SendBetInfo(users.Distinct().ToList());
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

                    try
                    {
                        var products = await db.Products.Include(x => x.PurchaseType).Include(x => x.ProductState)
                            .Where(x => x.PurchaseType.Type == PurchaseTypesConstant.AUCTION
                            && x.IsActive
                            && x.DateClose <= DateTime.UtcNow.AddMinutes(-1)).ToListAsync();

                        products.ForEach(x => x.IsActive = false);

                        var bets = await db.Purchases.Include(x => x.PurchaseState)
                            .Where(x => products.Select(p => p.Id).Contains(x.PoductId)).ToListAsync();

                        bets.ForEach(x => x.PurchaseStateId = closeId);

                        db.Products.UpdateRange(products);
                        db.Purchases.UpdateRange(bets);

                        await db.SaveChangesAsync();
                        await dbTransaction.CommitAsync();

                        var users = bets.Select(x => x.UserId).ToList();

                        if (users.Any())
                            await InvopkeBetIvent(users);

                        await Task.Delay(500);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
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
