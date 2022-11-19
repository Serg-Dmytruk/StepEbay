using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using StepEbay.Common.Helpers;
using StepEbay.Common.Models.ProductInfo;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Client.Common.ClientsHub;
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
        [Inject] private PriceHubClient PriceHubClient { get; set; }
        private ProductDto LastProd { get; set; }
        //[Inject]private TimezoneHelper TimezoneHelper { get; set; }
        private string ApiConnection { get; set; }
        ProductDto Product { get; set; } = new();
        PurchaseDto LastPurchase { get; set; } = new();
        private List<string> SrcPictures { get; set; } = new List<string>();

        private string currentPicture { get; set; } = "";

        public ProductPage()
        {
            Product = new();
            Product.ProductDescs = new Dictionary<string, string>();
        }

        private async void Buy()
        {
            var result = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.PlaceBet(int.Parse(Id)));
            if (result.StatusCode == HttpStatusCode.OK)
            {
                MessageService.ShowSuccsess($"Додано до кошику", $"Товар: {Product.Title} за ціною {Product.Price}"); ;
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
            Product.DateClosed = Product.DateClosed.AddHours(2);
            Product.DateCreated = Product.DateCreated.AddHours(2);
            //Product.DateClosed = await TimezoneHelper.ToLocalTime(Product.DateClosed);
            //Product.DateCreated = await TimezoneHelper.ToLocalTime(Product.DateCreated);

            if (SrcPictures.Count == 0)
            {
                currentPicture = Product.Image1;
                SrcPictures.Add(Product.Image1);
                SrcPictures.Add(Product.Image2);
                SrcPictures.Add(Product.Image3);
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
                    LastPurchase = new PurchaseDto { PurchasePrice = 0};
                }
            }

            StateHasChanged();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            PriceHubClient.ResetMyBetClosed();
            PriceHubClient.ChangedPriceSingle += ChangedPriceSingle;
        }

        private void ChangedPriceSingle(List<ChangedPrice> changed)
        {
            var changedProduct = changed.SingleOrDefault(c => c.ProductId == Product.Id);
            
            if (changedProduct is not null && (LastProd is null || (LastProd.Price != changedProduct.Price)))
            {
                LastProd = new ProductDto { Price = changedProduct.Price };
                Product.Price = changedProduct.Price;
                LastPurchase.PurchasePrice = changedProduct.Price;
                MessageService.ShowInfo("Ціна змінилася", $"{Product.Title} - {Math.Round(Product.Price+Product.Price * (decimal)0.02, 2)}");
            }
            StateHasChanged();
        }

        void ImageChanger(string src)
        {
            currentPicture = src;
        }
    }
}