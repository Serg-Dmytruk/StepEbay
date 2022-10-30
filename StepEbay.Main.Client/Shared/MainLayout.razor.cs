using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Common.ClientsHub;
using StepEbay.Main.Client.Common.Providers;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Bet;
using StepEbay.PushMessage.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StepEbay.Main.Client.Shared
{
    public partial class MainLayout
    {
        // [Inject] private LocalStorage LocalStorage { get; set; }
        [Inject] private PriceHubClient PriceHubClient { get; set; }
        [Inject] private HubClient HubClient { get; set; }
        [Inject] private ITokenProvider TokenProvider { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IMessageService MessageService { get; set; }
        [Inject] private IApiService ApiService { get; set; }
        private string UserName { get; set; }
        public bool IsShowSignModal { get; set; } = false;
        private string SearchRequest { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                HubClient.MyBetClosed += MyBetClosed;
                HubClient.OwnerClosed += OwnerClosed;
                HubClient.OwnerDeactivate += OwnerDeactivate;
            }

            //UserName = await LocalStorage.GetLocal("username");
            var authState = await TokenProvider.GetAuthenticationStateAsync();  //приклад витягування клаймів не ремувити)
            UserName = authState.User.Claims.FirstOrDefault(c => c.Type == "nickName")?.Value;

            if (!string.IsNullOrEmpty(UserName))
            {
                await HubClient.Start();
            }

            await PriceHubClient.Start();

            StateHasChanged();

        }

        private async void OwnerClosed(List<int> productInfo)
        {
            var products = (await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetProductInfo(new ProductInfoDto { ProductIds = productInfo }))).Data;

            foreach (var product in products)
                MessageService.ShowInfo($"ВАШ ТОВАР КУПЛЕНО", $"{product.Title} - {product.Price}, Час:{product.DateClosed}");
        }

        private async void MyBetClosed(List<int> productInfo)
        {
            var products = (await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetProductInfo(new ProductInfoDto { ProductIds = productInfo }))).Data;

            foreach (var product in products)
            {
                if (product.PurchaseTypeId == 2)
                    MessageService.ShowInfo($"АУКЦІОН ЗАВЕРШЕНО", $"{product.Title} - {product.Price}, Час:{product.DateClosed}");
                if (product.PurchaseTypeId == 1)
                    MessageService.ShowInfo($"ТОВАР ПРИДБАНО", $"{product.Title} - {product.Price}, Час:{product.DateClosed}");
            }
        }

        private async void OwnerDeactivate(List<int> ownerDeactivate)
        {
            var products = (await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetProductInfo(new ProductInfoDto { ProductIds = ownerDeactivate }))).Data;

            foreach (var product in products)
            {
                if (product.PurchaseTypeId == 2)
                    MessageService.ShowWarning($"ТОВАР ДЕАКТИВОВАНО ЗАВЕРШЕНО", $"{product.Title} - {product.Price}, Час:{product.DateClosed}");

            }
        }

        private void CloseSignModal(bool show)
        {
            IsShowSignModal = false;
            StateHasChanged();
        }

        private void ShowSignModal()
        {
            IsShowSignModal = true;
        }

        private async Task Search()
        {
            var response = (await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetSearchIds(SearchRequest))).Data;

            if (response.Ids.Count > 0)
            {
                NavigationManager.NavigateTo(uri:$"/products?{string.Join("&", response.Ids.Select(x => "product=" + x))}", forceLoad: true);    
            }
            else
            {
                NavigationManager.NavigateTo(uri: $"/products?product=0", forceLoad: true);
            }

            StateHasChanged();
        }
    }
}
