using Microsoft.AspNetCore.SignalR;
using StepEbay.Common.Models.ProductInfo;
using StepEbay.Main.Api.Common.Models.HubContainers;

namespace StepEbay.Main.Api.Common.Hubs
{
    public class BetHub : Hub
    {
        private readonly IHubContext<MainHub> _hubContext;
        private readonly HubUserContainer _hubUserContainer;

        public BetHub(IHubContext<MainHub> hubContext, HubUserContainer hubUserContainer)
        {
            _hubContext = hubContext;
            _hubUserContainer = hubUserContainer;
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
                await _hubContext.Clients.Clients(user).SendAsync("OwnerClosed", owners.Where(x => x.UserId == _hubUserContainer.Users[user]).Select(x => x.ProductId).ToList());
            }
        }
    }
}
