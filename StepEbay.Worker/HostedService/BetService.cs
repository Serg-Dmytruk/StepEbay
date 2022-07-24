using Microsoft.Extensions.Hosting;
using StepEbay.Worker.ClientHubs;

namespace StepEbay.Worker.HostedService
{
    public class BetService : BackgroundService
    {
        private readonly BetHubClient _betHubClient;

        public BetService(BetHubClient betHubClient)
        {
            _betHubClient = betHubClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await StartScanningBets();
        }

        private async Task StartScanningBets()
        {
            //select from db logic
        }

        private async Task InvopkeBetIvent(List<int> users)
        {
            await _betHubClient.SendBetInfo(users.Distinct().ToList());
        }
    }
}
