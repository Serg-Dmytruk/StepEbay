using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using StepEbay.Common.Helpers;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Bet;
using StepEbay.Main.Common.Models.Product;
using StepEbay.PushMessage.Services;
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
        //[Inject]private TimezoneHelper TimezoneHelper { get; set; }
        private string ApiConnection { get; set; }
        ProductDto Product { get; set; }
        PurchaseDto LastPurchase { get; set; }
        private List<string> SrcPictures { get; set; } = new List<string>();

        private string currentPicture { get; set; } = "";

        public ProductPage()
        {
            Product = new();
        }

        private async void Buy()
        {
            var result = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.PlaceBet(int.Parse(Id)));
            if (result.StatusCode == HttpStatusCode.OK)
            {
                MessageService.ShowSuccsess($"Додано до кошику", "Товар: "+Product.Title+" за ціною "+Product.Price);
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
            Product.DateClosed = Product.DateClosed;
            Product.DateCreated = Product.DateCreated;
            //Product.DateClosed = await TimezoneHelper.ToLocalTime(Product.DateClosed);
            //Product.DateCreated = await TimezoneHelper.ToLocalTime(Product.DateCreated);

            if (SrcPictures.Count == 0)
            {
                currentPicture = Product.Image;
                SrcPictures.Add(Product.Image);
                SrcPictures.Add("test1.png");
                SrcPictures.Add("test2.png");
            }
                

            if (Product.PurchaseTypeId == 2)
            {
                ResponseData<List<PurchaseDto>> resultPurchases = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetPurchase(int.Parse(Id)));
                if (resultPurchases.Data.Count > 0)
                {
                    LastPurchase = resultPurchases.Data.Last();
                }
                else
                {
                    LastPurchase = null;
                }
            }

            StateHasChanged();
        }

        void ImageChanger(string src)
        {
            currentPicture = src;
        }
    }
}