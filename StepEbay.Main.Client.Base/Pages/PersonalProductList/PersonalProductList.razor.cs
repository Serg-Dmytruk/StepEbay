using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using StepEbay.Common.Models.Pagination;
using StepEbay.Main.Client.Base.Layout;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Bet;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Client.Base.Pages.PersonalProductList
{
    [Route("personal/products")]
    [Authorize]
    [Layout(typeof(EmptyLayout))]
    public partial class PersonalProductList
    {
        [Inject] IApiService ApiService { get; set; }

        private PaginatedList<ProductDto> _products = new PaginatedList<ProductDto>();
        private BetFilter _filters = new BetFilter();
        public int ProductPageNumber = 0;
        public int MaxProductPageNumber = 0;
        private readonly int _productOnPageNumber = 3;

        protected override async Task OnInitializedAsync()
        {
            _filters.Active = true;
            _filters.Closed = true;
            _products.List = new List<ProductDto>();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //ShowPreloader = true;
            if (firstRender)
            {
                await GetAllUserProducts();
            }

            StateHasChanged();
            //ShowPreloader = false;
        }

        public async Task GetAllUserProducts()
        {
            var responce = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetPersonalProduct(ProductPageNumber, _filters.Active, _filters.Closed));
            _products = responce.Data;
        }
    }
}
