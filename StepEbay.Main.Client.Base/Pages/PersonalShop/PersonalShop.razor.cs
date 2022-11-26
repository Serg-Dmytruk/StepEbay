using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using StepEbay.Common.Models.Pagination;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Bet;
using StepEbay.Main.Common.Models.Product;
using StepEbay.PushMessage.Services;

namespace StepEbay.Main.Client.Base.Pages.PersonalShop
{
    [Route("/personal/shop")]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public partial class PersonalShop
    {
        [Inject] private IConfiguration Configuration { get; set; }
        [Inject] private IApiService ApiService { get; set; }

        private string ApiConnection { get; set; }
        private PaginatedList<ProductDto> _products = new PaginatedList<ProductDto>();
        private BetFilter _filters = new BetFilter();
        public int ProductPageNumber = 0;
        public int MaxProductPageNumber = 0;

        protected override async Task OnInitializedAsync()
        {
            _filters.Active = true;
            _filters.Closed = true;
            _products.List = new List<ProductDto>();
            await GetAllUserProducts();

            ApiConnection = Configuration.GetConnectionString("Api");
            StateHasChanged();
        }

        public async Task GetAllUserProducts()
        {
            var responce = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetPersonalShopProducts(ProductPageNumber));
            _products = responce.Data;
        }
    }
}
