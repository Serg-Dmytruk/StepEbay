using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Refit;
using StepEbay.Common.Helpers;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Models.Products;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Product;
using StepEbay.PushMessage.Services;
using System;
using System.Net;

namespace StepEbay.Main.Client.Base.Pages.Products
{
    [Route("/product/{id}")]
    public partial class ProductPage
    {
        [Parameter] public string Id { get; set; }
        [Inject] private IApiService ApiService { get; set; }
        [Inject] IMessageService MessageService { get; set; }
        [Inject] private IConfiguration Configuration { get; set; }
        [Inject]private TimezoneHelper TimezoneHelper { get; set; }
        private string ApiConnection { get; set; }
        ProductDto Product { get; set; }

        public ProductPage()
        {
            Product = new();
        }

        private async void Buy()
        {
            var result = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.PlaceBet(int.Parse(Id)));
            if (result.StatusCode == HttpStatusCode.OK)
            {
                MessageService.ShowSuccsess($"Додано до кошику", "Товар: {Product.Title} за ціною {Product.Price}");
            }
            else if (result.StatusCode == HttpStatusCode.Unauthorized)
            {
                MessageService.ShowError("Відміна", "Потрібно авторизуватись");
            }
            else
            {
                MessageService.ShowError("Помилка", result.Errors.First().Value.First());
            }
        }

        protected override async Task OnInitializedAsync()
        {
            ApiConnection = Configuration.GetConnectionString("Api");
            ResponseData<ProductDto> result = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetProduct(int.Parse(Id)));

            if (result.StatusCode != HttpStatusCode.OK)
                MessageService.ShowError("Помилка", result.Errors.First().Value.First());

            Product = result.Data;
            Product.DateClosed = await TimezoneHelper.ToLocalTime(Product.DateClosed);
            Product.DateCreated = await TimezoneHelper.ToLocalTime(Product.DateCreated);

            StateHasChanged();
        }
    }
}