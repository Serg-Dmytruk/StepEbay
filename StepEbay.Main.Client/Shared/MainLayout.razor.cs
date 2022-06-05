using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Common.Providers;
using System.Threading.Tasks;
using System.Linq;

namespace StepEbay.Main.Client.Shared
{
    public partial class MainLayout
    {
        [Inject] private ITokenProvider TokenProvider { get; set; }
        private string UserName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await TokenProvider.GetAuthenticationStateAsync();
            UserName = authState.User.Claims.FirstOrDefault(c => c.Type == "nickName")?.Value;
        }

        private async Task Logout()
        {
            await TokenProvider.RemoveToken();
            await TokenProvider.CheckAuthentication(false);
        }
    }
}
