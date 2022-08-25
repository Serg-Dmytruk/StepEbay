using Microsoft.AspNetCore.Components;
using StepEbay.Common.Storages;
using StepEbay.Main.Client.Common.Providers;
using System.Threading.Tasks;
using System.Linq;
using StepEbay.Main.Client.Common.ClientsHub;

namespace StepEbay.Main.Client.Shared
{
    public partial class MainLayout
    {
        // [Inject] private LocalStorage LocalStorage { get; set; }
        [Inject] private HubClient HubClient { get; set; }
        [Inject] private ITokenProvider TokenProvider { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        private string UserName { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //UserName = await LocalStorage.GetLocal("username");
            var authState = await TokenProvider.GetAuthenticationStateAsync();  //приклад витягування клаймів не ремувити)
            UserName = authState.User.Claims.FirstOrDefault(c => c.Type == "nickName")?.Value;

            if(!string.IsNullOrEmpty(UserName))
            {
                await HubClient.Start();
            }
            
            StateHasChanged();
        }

        private async Task Logout()
        {
            await TokenProvider.RemoveToken();
            await TokenProvider.CheckAuthentication(false);
            
            NavigationManager.NavigateTo("/");
        }

        private async Task LogIn()
        {

        }
    }
}
