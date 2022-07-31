using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            await Task.WhenAll(Task.Run(() => BetInfoClosed(), stoppingToken),
                    Task.Run(() => CloseBets(), stoppingToken));
        }

        private async Task InvopkeBetIvent(List<int> users)
        {
            await _betHubClient.SendBetInfo(users.Distinct().ToList());
        }

        private  async Task CloseBets()
        {
            while (true)
            {

                using var scope = _services.CreateScope();
                {

                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await using var dbTransaction = await db.Database.BeginTransactionAsync();

                    try
                    {

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

        private async Task BetInfoClosed()
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
