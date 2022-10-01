using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using StepEbay.Common.Models.ProductInfo;
using StepEbay.Main.Client.Common.Providers;

namespace StepEbay.Main.Client.Common.ClientsHub
{
    public class PriceHubClient
    {
        private readonly HubConnection _connection;
        public PriceHubClient(IConfiguration config, ITokenProvider tokenProvider)
        {
            _connection = new HubConnectionBuilder()
            .WithUrl(new Uri(config.GetConnectionString("PriceHub")),
             options => { options.AccessTokenProvider = async () => await tokenProvider.GetToken(); })
            .WithAutomaticReconnect()
            .Build();

            _connection.KeepAliveInterval = new TimeSpan(0, 0, 1);

            _connection.On<List<ChangedPrice>>("ChangedPrice", value => ChangedPrice.Invoke(value));
          
            _connection.Closed += async error =>
            {
                await Task.Delay(1000);
                await _connection.StartAsync();
            };
        }

        public event Action<List<ChangedPrice>> ChangedPrice;

        public async Task Start()
        {
            try
            {
                if (_connection.State != HubConnectionState.Connected || _connection.State != HubConnectionState.Connecting)
                    await _connection.StartAsync();
            }
            catch
            {
            }
        }

        public async Task Stop()
        {
            try
            {
                if (_connection.State != HubConnectionState.Disconnected)
                    await _connection.StopAsync();
            }
            catch
            {
            }
        }
    }
}
