using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using StepEbay.Common.Models.Pagination;
using StepEbay.Common.Models.ProductInfo;
using StepEbay.Main.Client.Common.ClientsHub;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Product;
using StepEbay.Common.Constans;

namespace StepEbay.Main.Client.Base.Pages
{
    [Route("products")]
    [Route("products/{filter}")]
    public partial class ProductList
    {
        [Parameter] public string filter { get; set; }
        [Inject] IApiService ApiService { get; set; }
        [Inject] private IConfiguration Configuration { get; set; }
        [Inject] private PriceHubClient PriceHubClient { get; set; }
        public bool ShowModal { get; set; } = false;

        private Dictionary<string, List<string>> MessageConfirmReg = new();
        private bool ShowPreloader { get; set; } = false;
        private List<CategoryDto> _categories = new();
        private PaginatedList<ProductDto> _products = new();
        private string ApiConnection { get; set; }
        private ProductFilters ProductFilters { get; set; } = new ProductFilters();

        public int ProductPageNumber = 0;
        public int MaxProductPageNumber = 0;

        protected override void OnInitialized()
        {
            ApiConnection = Configuration.GetConnectionString("Api");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            ShowPreloader = true;
            if (firstRender)
            {
                PriceHubClient.ChangedPrice += ChangedPrice;

                await GetCategories();
                await GetProductStates();
                SetDefaultFilters(_categories);
                await SubmitFilters();
            }
            ShowPreloader = false;
            StateHasChanged();
            
        }

        private void CloseModal(bool show)
        {
            ShowModal = show;
            StateHasChanged();
        }

        protected async void PrevProductPage()
        {
            if (ProductPageNumber != 0)
            {
                ProductPageNumber -= 1;
                await SubmitFilters();
            }
        }

        protected async void NextProductPage()
        {
            if (ProductPageNumber != (MaxProductPageNumber - 1))
            {
                ProductPageNumber += 1;
                await SubmitFilters();
            }
        }

        protected async Task GetCategories()
        {
            var responce = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetCategories());
            _categories = responce.Data;
        }

        protected async Task SubmitFilters()
        {
            ShowPreloader = true;
            List<int> categories = new();
            foreach (var category in ProductFilters.Categories)
            {
                if (category.Selected is true)
                    categories.Add(category.Id);
            }

            List<int> states = new();
            foreach (var state in ProductFilters.States)
            {
                if (state.Selected is true)
                    states.Add(state.Id);
            }

            var responce = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetProductsWithFilters(new ProductFilterInfo
            { Categories = categories, States = states, PriceStart = ProductFilters.PriceStart, PriceEnd = ProductFilters.PriceEnd }, ProductPageNumber));
            _products = responce.Data;

            if (_products is not null)
            {
                MaxProductPageNumber = _products.CountAll % ProductListConstant.MAXONPAGE == 0 ? (_products.CountAll / ProductListConstant.MAXONPAGE) : (_products.CountAll / ProductListConstant.MAXONPAGE) + 1;
            }
            ShowPreloader = false;
        }

        protected void SetDefaultFilters(List<CategoryDto> categories)
        {
            if (filter == "0")
            {
                categories.ForEach(category => ProductFilters.Categories.Add(new Category { Id = category.Id, Name = category.Name, Selected = true }));
            }
            else
            {
                foreach (var category in categories)
                {
                    if (!string.IsNullOrEmpty(filter) && category.Id.ToString() == filter)
                        ProductFilters.Categories.Add(new Category { Id = category.Id, Name = category.Name, Selected = true });
                    else
                        ProductFilters.Categories.Add(new Category { Id = category.Id, Name = category.Name, Selected = false });
                }
            }

        }

        protected async Task GetProductStates()
        {
            var responce = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetProductStates());
            ProductFilters.States = responce.Data.Select(r => new State { Id = r.Id, Name = r.Name, Selected = true }).ToList();
        }

        private void ChangedPrice(List<ChangedPrice> changed)
        {
            _products.List.ForEach(x =>
            {
                var changedProduct = changed.SingleOrDefault(c => c.ProductId == x.Id);

                if (changedProduct is not null)
                    x.Price = changedProduct.Price;
            });
        }
    }
}
