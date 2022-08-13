using Microsoft.AspNetCore.Authorization;
using StepEbay.Main.Api.Common.Models.HubContainers;

namespace StepEbay.Main.Api.Common.Hubs
{
    [Authorize]
    public class MainHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly HubUserContainer _hubUserContainer;

        public MainHub(HubUserContainer hubUserContainer)
        {
            _hubUserContainer = hubUserContainer;
        }

        public override async Task OnConnectedAsync()
        {
            _hubUserContainer.Users.TryAdd(Context.ConnectionId, int.Parse(Context.User.Identity.Name));
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            _hubUserContainer.Users.TryRemove(Context.ConnectionId, out _);
        }
    }
}
