using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Configuration;
using Refit;
using StepEbay.Common.Helpers;
using StepEbay.Common.Models.Pagination;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Bet;
using StepEbay.Main.Common.Models.Product;
using StepEbay.PushMessage.Services;
using System.Net;

namespace StepEbay.Main.Client.Base.Pages.Products
{
    [Route("/add/product")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public partial class AddProduct
    {
        [Inject] private IConfiguration Configuration { get; set; }
        [Inject] private IApiService ApiService { get; set; }
        [Inject] IMessageService MessageService { get; set; }

        private PaginatedList<ProductDto> _products = new PaginatedList<ProductDto>();
        private BetFilter _filters = new BetFilter();
        public int ProductPageNumber = 0;
        public int MaxProductPageNumber = 0;
        private List<CategoryDto> Categories { get; set; }
        private List<ProductStateDto> States { get; set; }
        private List<PurchaseTypeResponseDto> Types { get; set; }
        private string ApiConnection { get; set; }

        private readonly Dictionary<string, object> LogoAttributes = new()
        {
            {"id", "file-goods-add"}
        };

        private string PictureFileName { get; set; }

        private ProductDto request = new ProductDto();

        public AddProduct()
        {
            Categories = new List<CategoryDto>();
            States = new List<ProductStateDto>();
            Types = new List<PurchaseTypeResponseDto>();
        }

        protected override async Task OnInitializedAsync()
        {
            _filters.Active = true;
            _filters.Closed = true;
            _products.List = new List<ProductDto>();
            await GetAllUserProducts();

            ApiConnection = Configuration.GetConnectionString("Api");
            Categories = (await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetCategories())).Data;
            States = (await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetProductStates())).Data;
            Types = (await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetAllPurchaseTypes())).Data;

            request.CategoryId = Categories.FirstOrDefault().Id;
            request.StateId = States.FirstOrDefault().Id;
            request.PurchaseTypeId = Types.FirstOrDefault().Id;

            StateHasChanged();
        }

        private async void Submith()
        {
            Random rand = new();
            request.Rate =rand.Next(3,6);
            var result = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.AddProduct(request));

            if (result.StatusCode != HttpStatusCode.OK)
                MessageService.ShowError("Помилка", result.Errors.First().Value.First());
            else
            {
                MessageService.ShowSuccsess("Успіх!", "Товар додано");
                ClearFields();
            }
        }

        private async Task UploadImage(InputFileChangeEventArgs e)
        {
            if (e.FileCount != 0)
            {
                PictureFileName = "Загрузка...";

                var image = e.File;
                var stream = image.OpenReadStream(ByteHelper.ConvertMegabytesToBytes(20));
                MemoryStream memoryStream = new();
                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                stream.Close();

                StreamPart streamPart = new(memoryStream, image.Name);
                request.Image = (await ApiService.ExecuteRequest(() => ApiService.ApiMethods.UploadImage(streamPart)))
                    .Data.FileName;

                memoryStream.Close();
                PictureFileName = image.Name;
            }
        }

        public async Task GetAllUserProducts()
        {
            var responce = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetPersonalProduct(ProductPageNumber, _filters.Active, _filters.Closed));
            _products = responce.Data;
        }

        private void ClearFields()
        {
            request = new();
            StateHasChanged();
        }
    }
}