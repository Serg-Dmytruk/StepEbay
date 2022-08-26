using Microsoft.AspNetCore.Components;
using Refit;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Person;
using StepEbay.Main.Common.Models.Product;
using System.Xml;

namespace StepEbay.Main.Client.Base.Pages.Products
{
    [Route("/add/product")]
    public partial class AddProduct
    {
        [Inject] IApiService _apiService { get; set; }
        List<CategoryResponseDto> _categories { get; set; }
        List<ProductStateResponseDto> _states { get; set; }

        private string _image;
        private string _title;
        private string _description;
        private decimal _price;
        private bool _byNow;
        private int _count;
        private int _categoryId;
        private int _stateId;

        private string message;

        public AddProduct()
        {
            _categories = new List<CategoryResponseDto>();
            _states= new List<ProductStateResponseDto>();
        }

        protected override async Task OnInitializedAsync()
        {
            _categories = (await _apiService.ExecuteRequest(() => _apiService.ApiMethods.GetAllCategorys())).Data;
            _states= (await _apiService.ExecuteRequest(() => _apiService.ApiMethods.GetAllStates())).Data;

            this.StateHasChanged();
        }

        private async void Submith()
        {
            ProductRequestDto request = new ProductRequestDto()
            {
                DateCreated = DateTime.Now,
                Image = _image,
                Title = _title,
                Description = _description,
                Price = _price,
                ByNow = _byNow,
                Count=_count,
                IdCategory=_categoryId,
                IdProductState=_stateId
            };

            ResponseData<BoolResult> result=await _apiService.ExecuteRequest(() => _apiService.ApiMethods.AddProduct(request));
            
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
            _count= 0;
            _categoryId = 0;
            _stateId = 0;
            this.StateHasChanged();
        }
    }
}