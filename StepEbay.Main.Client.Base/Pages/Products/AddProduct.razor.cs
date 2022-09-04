using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Product;
using System.Net;

namespace StepEbay.Main.Client.Base.Pages.Products
{
    [Route("/add/product")]
    [Authorize]
    public partial class AddProduct
    {
        [Inject] private IApiService _apiService { get; set; }
        private List<CategoryDto> _categories { get; set; }
        private List<ProductStateDto> _states { get; set; }
        private List<PurchaseTypeResponseDto> _types { get; set; }

        private ProductDto request = new ProductDto();

        private string message;

        public AddProduct()
        {
            _categories = new List<CategoryDto>();
            _states = new List<ProductStateDto>();
            _types = new List<PurchaseTypeResponseDto>();
        }

        protected override async Task OnInitializedAsync()
        {
            _categories = (await _apiService.ExecuteRequest(() => _apiService.ApiMethods.GetCategories())).Data;
            _states = (await _apiService.ExecuteRequest(() => _apiService.ApiMethods.GetProductStates())).Data;
            _types = (await _apiService.ExecuteRequest(() => _apiService.ApiMethods.GetAllPurchaseTypes())).Data;

            this.StateHasChanged();
        }

        private async void Submith()
        {
            var result = await _apiService.ExecuteRequest(() => _apiService.ApiMethods.AddProduct(request));

            if (result.StatusCode != HttpStatusCode.OK)
                message = result.Errors.First().Value.First();
            else
                message = "Advierment added";
        }
        private void ClearFields()
        {
            request = new();
            StateHasChanged();
        }
    }
}