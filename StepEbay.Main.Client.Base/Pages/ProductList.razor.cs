using Microsoft.AspNetCore.Components;
using StepEbay.Common.Models.Pagination;
using StepEbay.Main.Client.Base.Layout;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Client.Base.Pages
{
    [Route("products")]
    [Route("products/{filter}")]
    [Layout(typeof(EmptyLayout))]
    public partial class ProductList
    {
        [Parameter] public string filter { get; set; }
        [Inject] IApiService ApiService { get; set; }

        private bool ShowPreloader { get; set; } = false;
        private List<CategoryDto> _categories = new List<CategoryDto>();
        private PaginatedList<ProductDto> _products = new PaginatedList<ProductDto>();
        private ProductFilters _productFilters { get; set; } = new ProductFilters();

        public int ProductPageNumber = 0;
        public int MaxProductPageNumber = 0;
        private readonly int _productOnPageNumber = 3;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //ShowPreloader = true;
            if (firstRender)
            {
                await GetProducts();
                await GetCategories();
                await GetProductStates();
                SetDefaultFilters(_categories);
            }

            StateHasChanged();
            //ShowPreloader = false;
        }

        protected void PrevProductPage()
        {
            if (ProductPageNumber != 0)
                ProductPageNumber -= 1;
        }

        protected void NextProductPage()
        {
            if (ProductPageNumber != (MaxProductPageNumber - 1))
                ProductPageNumber += 1;
        }

        protected async Task GetCategories()
        {
            var responce = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetCategories());
            _categories = responce.Data;
        }

        protected async Task SubmitFilters()
        {
            List<int> categories = new();
            foreach(var category in _productFilters.Categories)
            {
                if(category.Selected is true)
                    categories.Add(category.Id);
            }

            List<int> states = new();
            foreach (var state in _productFilters.States)
            {
                if (state.Selected is true)
                    states.Add(state.Id);
            }

            var responce = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetProductsWithFilters( new ProductFilterInfo 
                { Categories = categories, States = states, PriceStart = _productFilters.PriceStart, PriceEnd = _productFilters.PriceEnd}, ProductPageNumber));
            _products = responce.Data;

            if (_products is not null)
                MaxProductPageNumber = (_products.CountAll / _productOnPageNumber);
        }

        protected void SetDefaultFilters(List<CategoryDto> categories)
        {
            foreach (var category in categories)
            {
                if (!string.IsNullOrEmpty(filter) && category.Id.ToString() == filter)
                    _productFilters.Categories.Add(new Category { Id = category.Id, Name = category.Name, Selected = true });
                else
                    _productFilters.Categories.Add(new Category { Id = category.Id, Name = category.Name, Selected = false });
            }
        }

        protected async Task GetProducts()
        {
            var responce = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetProducts(ProductPageNumber, filter));
            _products = responce.Data;

            if (_products is not null)
                MaxProductPageNumber = (_products.CountAll / _productOnPageNumber);
        }

        protected async Task GetProductStates()
        {
            var responce = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetProductStates());
            _productFilters.States = responce.Data.Select(r => new State { Id = r.Id, Name = r.Name, Selected = true}).ToList();
        }
    }
}
