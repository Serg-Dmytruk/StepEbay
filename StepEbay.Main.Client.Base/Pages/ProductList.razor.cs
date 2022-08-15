using Microsoft.AspNetCore.Components;
using StepEbay.Common.Models.Pagination;
using StepEbay.Main.Client.Base.Layout;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Client.Base.Pages
{
    [Route("productList")]
    [Route("productList/{filter}")]
    [Layout(typeof(EmptyLayout))]
    public partial class ProductList
    {
        [Parameter] public string filter { get; set; }
        [Inject] IApiService ApiService { get; set; }

        private ProductFilters _productFilters { get; set; } = new ProductFilters();
        private bool ShowPreloader { get; set; } = false;
        private List<CategoryDto> _categories = new List<CategoryDto>();
        private PaginatedList<ProductDto> _products = new PaginatedList<ProductDto>();

        public int ProductPageNumber = 0;
        public int MaxProductPageNumber = 0;
        private readonly int _productOnPageNumber = 3;

        protected override async Task OnInitializedAsync()
        {

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //ShowPreloader = true;
            await GetProducts();
            await GetCategories();

            if (firstRender)
            {
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

        protected void OnFilterMouseDown(int a)
        {
            var category = _productFilters.categories.SingleOrDefault(c => c.id == a);
            if (category is not null)
            {
                category.selected = !category.selected;
            }
            StateHasChanged();
        }

        protected async Task SubmitFilters()
        {

        }

        protected void SetDefaultFilters(List<CategoryDto> categories)
        {
            foreach (var category in categories)
            {
                _productFilters.categories.Add(new Category { id = category.Id, name = category.Name, selected = false });
            }
        }

        protected async Task GetProducts()
        {
            var responce = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetProducts(ProductPageNumber));
            _products = responce.Data;

            if (_products is not null)
                MaxProductPageNumber = (_products.CountAll / _productOnPageNumber);
        }
    }
}
