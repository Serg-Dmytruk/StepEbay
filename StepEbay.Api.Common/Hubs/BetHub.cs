using Microsoft.AspNetCore.SignalR;
using StepEbay.Common.Models.ProductInfo;
using StepEbay.Main.Api.Common.Models.HubContainers;

namespace StepEbay.Main.Api.Common.Hubs
{
    public class BetHub : Hub
    {
        private readonly IHubContext<MainHub> _hubContext;
        private readonly IHubContext<PriceHub> _priceHubContext;
        private readonly HubUserContainer _hubUserContainer;
        private readonly PriceUserContainer _priceUserContainer;

        public BetHub(IHubContext<MainHub> hubContext,
            IHubContext<PriceHub> priceHubContext,
            HubUserContainer hubUserContainer,
            PriceUserContainer priceUserContainer)
        {
            _hubContext = hubContext;
            _priceHubContext = priceHubContext;
            _hubUserContainer = hubUserContainer;
            _priceUserContainer = priceUserContainer;
        }

        public async Task MyBetClosed(List<ProductInfo> users)
        {
            var connectedUsers = _hubUserContainer.Users.Where(x => users.Select(x => x.UserId).Contains(x.Value)).Select(x => x.Key);

            foreach (var user in connectedUsers)
            {
                await _hubContext.Clients.Clients(user).SendAsync("MyBetClosed", users.Where(x => x.UserId == _hubUserContainer.Users[user]).Select(x => x.ProductId).ToList());
            }    
        }

        public async Task OwnerClosed(List<ProductInfo> owners)
        {
            var connectedUsers = _hubUserContainer.Users.Where(x => owners.Select(x => x.UserId).Contains(x.Value)).Select(x => x.Key);

            foreach (var user in connectedUsers)
            {
                await _hubContext.Clients.Clients(connectedUsers).SendAsync("OwnerClosed", owners.Where(x => x.UserId == _hubUserContainer.Users[user]).Select(x => x.ProductId).ToList());
            }
        }

        public async Task OwnerDeactivate(List<ProductInfo> owners)
        {
            var connectedUsers = _hubUserContainer.Users.Where(x => owners.Select(x => x.UserId).Contains(x.Value)).Select(x => x.Key);

            foreach (var user in connectedUsers)
            {
                await _hubContext.Clients.Clients(user).SendAsync("OwnerDeactivate", owners.Where(x => x.UserId == _hubUserContainer.Users[user]).Select(x => x.ProductId).ToList());
            }
        }

        public async Task ChangedPrice(List<ChangedPrice> prices)
        {
            await _priceHubContext.Clients.Clients(_priceUserContainer.Users).SendAsync("ChangedPrice", prices);
        }

        public async Task ChangedPriceSingle(List<ChangedPrice> prices)
        {
            await _priceHubContext.Clients.Clients(_priceUserContainer.Users).SendAsync("ChangedPriceSingle", prices);
        }
    }
}
