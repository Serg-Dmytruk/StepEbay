using Microsoft.AspNetCore.SignalR;
using StepEbay.Main.Api.Common.Models.HubContainers;

namespace StepEbay.Main.Api.Common.Hubs
{
    public class PriceHub : Hub
    {
        private readonly PriceUserContainer _priceUserContainer;

        public PriceHub(PriceUserContainer priceUserContainer)
        {
            _priceUserContainer = priceUserContainer;
        }

        public override async Task OnConnectedAsync()
        {
            _priceUserContainer.Users.Add(Context.ConnectionId);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _priceUserContainer.Users.Remove(Context.ConnectionId);
        }
    }
}
