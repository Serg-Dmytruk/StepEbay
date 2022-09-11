using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace StepEbay.Worker.ClientHubs
{
    public class BetHubClient
    {
        private readonly HubConnection _connection;
        private readonly ILogger<BetHubClient> _logger;
        public BetHubClient(IConfiguration configuration, ILogger<BetHubClient> logger)
        {
            _logger = logger;
            _connection = new HubConnectionBuilder()
            .WithUrl(new Uri(configuration.GetConnectionString("Bet")))
            .WithAutomaticReconnect()
            .Build();

            _connection.KeepAliveInterval = new TimeSpan(0, 0, 1);

            _connection.Closed += async error =>
            {
                await Task.Delay(2000);
                _logger.LogError(error.Message);
                await _connection.StartAsync();
            };
        }

        /// <summary>
        /// Підключення до хаба що в основному проекті
        /// </summary>
        public async Task Start()
        {
            try
            {
                await _connection.StartAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// Повідомлення про подію завершення аукціону
        /// </summary>
        public async  Task SendBetInfo(List<int> users)
        {
            await _connection.InvokeAsync("MyBetClosed", users);
        }

        /// <summary>
        /// Повідомлення власнику товара що аукціон чи купівля завершена
        /// </summary>
        public async Task SendOwnerInfo(List<int> owners)
        {
            await _connection.InvokeAsync("OwnerClosed", owners);
        }
    }
}
