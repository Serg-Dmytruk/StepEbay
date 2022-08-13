using Microsoft.AspNetCore.SignalR;
using StepEbay.Main.Api.Common.Models.HubContainers;

namespace StepEbay.Main.Api.Common.Hubs
{
    public class BetHub : Hub
    {
        private readonly IHubContext<BetHub> _hubContext;
        private readonly HubUserContainer _hubUserContainer;

        public BetHub(IHubContext<BetHub> hubContext, HubUserContainer hubUserContainer)
        {
            _hubContext = hubContext;
            _hubUserContainer = hubUserContainer;
        }

        public async Task MyBetClosed(List<int> users)
        {
            await _hubContext.Clients.Clients(
                _hubUserContainer.Users.Where(x => users.Contains(x.Value)).Select(x => x.Key)).SendAsync("MyBetClosed");
        }
    }
}
