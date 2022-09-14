using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Common.ClientsHub;
using StepEbay.Main.Client.Common.Providers;
using StepEbay.PushMessage.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StepEbay.Main.Client.Shared
{
    public partial class MainLayout
    {
        // [Inject] private LocalStorage LocalStorage { get; set; }
        [Inject] private HubClient HubClient { get; set; }
        [Inject] private ITokenProvider TokenProvider { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IMessageService MessageService { get; set; }
        private string UserName { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                HubClient.MyBetClosed += MyBetClosed;
                HubClient.OwnerClosed += OwnerClosed;
            }

            //UserName = await LocalStorage.GetLocal("username");
            var authState = await TokenProvider.GetAuthenticationStateAsync();  //приклад витягування клаймів не ремувити)
            UserName = authState.User.Claims.FirstOrDefault(c => c.Type == "nickName")?.Value;

            if (!string.IsNullOrEmpty(UserName))
            {
                await HubClient.Start();
            }

            StateHasChanged();

        }

        private async Task Logout()
        {
            await TokenProvider.RemoveToken();
            await TokenProvider.CheckAuthentication(false);

            await HubClient.Stop();

            NavigationManager.NavigateTo("/");
        }

        private async Task LogIn()
        {

        }

        private async Task OwnerClosed()
        {
            MessageService.ShowInfo("ТОВАР КУПЛУНО", "НАЗВА ТОВАРУ коли купили");
        }

        private async Task MyBetClosed()
        {
            MessageService.ShowInfo("АУКЦІОН ЗАВЕРШЕНО", "НАЗВА ТОВАРУ програв чи виграв");
        }
    }
}
