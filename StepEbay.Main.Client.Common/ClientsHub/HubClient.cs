﻿using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using StepEbay.Main.Client.Common.Providers;

namespace StepEbay.Main.Client.Common.ClientsHub
{
    public class HubClient
    {
        private readonly HubConnection _connection;
        public HubClient(IConfiguration config, ITokenProvider tokenProvider)
        {
            _connection = new HubConnectionBuilder()
            .WithUrl(new Uri(config.GetConnectionString("MainHub")), 
             options => { options.AccessTokenProvider = async () => await tokenProvider.GetToken(); })
            .WithAutomaticReconnect()
            .Build();

            _connection.KeepAliveInterval = new TimeSpan(0, 0, 1);

            _connection.On<List<int>>("MyBetClosed", value => MyBetClosed.Invoke(value));
            _connection.On<List<int>>("OwnerClosed", value => OwnerClosed.Invoke(value));

            _connection.Closed += async error =>
            {
                await Task.Delay(1000);
                await _connection.StartAsync();
            };
        }

        public event Action<List<int>> MyBetClosed;
        public event Action<List<int>> OwnerClosed;

        public async Task Start()
        {
            try
            {
                if(_connection.State != HubConnectionState.Connected)
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
