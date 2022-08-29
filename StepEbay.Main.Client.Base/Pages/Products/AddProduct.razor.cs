using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Client.Base.Pages.Products
{
    [Route("/add/product")]
    [Authorize]
    public partial class AddProduct
    {
        [Inject] IApiService _apiService { get; set; }
        List<CategoryDto> _categories { get; set; }
        List<ProductStateDto> _states { get; set; }
        List<PurchaseTypeResponseDto> _types { get; set; }

        private string _image;
        private string _title;
        private string _description;
        private decimal _price;
        private bool _byNow;
        private int _count;
        private int _categoryId;
        private int _stateId;
        private int _typeId;

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
            ProductDto request = new ProductDto()
            {
                Image = _image,
                Title = _title,
                Description = _description,
                Price = _price,
                ByNow = _byNow,
                Count = _count,
                CategoryId = _categoryId,
                StateId = _stateId,
                PurchaseTypeId = _typeId
            };

            var result = await _apiService.ExecuteRequest(() => _apiService.ApiMethods.AddProduct(request));

            if (result.Data.Value)
            {
                message = "Advierment added";
                ClearFields();
            }
            else
            {
                message = result.Data.ErrorMessage;
            }
        }
        private void ClearFields()
        {
            _image = "";
            _title = "";
            _description = "";
            _price = 0;
            _byNow = false;
            _count = 0;
            _categoryId = 0;
            _stateId = 0;
            this.StateHasChanged();
        }
    }
}