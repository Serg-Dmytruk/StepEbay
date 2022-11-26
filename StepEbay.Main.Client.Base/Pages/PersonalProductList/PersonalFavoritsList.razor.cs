﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using StepEbay.Common.Constans;
using StepEbay.Common.Models.Pagination;
using StepEbay.Common.Models.ProductInfo;
using StepEbay.Main.Client.Common.ClientsHub;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Bet;
using StepEbay.Main.Common.Models.Product;
using StepEbay.PushMessage.Services;
using System.Collections.Generic;

namespace StepEbay.Main.Client.Base.Pages.PersonalProductList
{
    [Route("personal/favorite")]
    [Route("personal/favorite/{filter}")]
    [Authorize]
    public partial class PersonalFavoritsList
    {
        [Parameter]
        [SupplyParameterFromQuery(Name = "product")]
        public int[]? SearchResult { get; set; }
        [Parameter] public string Filter { get; set; }
        [Inject] private IMessageService MessageService { get; set; }
        [Inject] IApiService ApiService { get; set; }
        [Inject] private IConfiguration Configuration { get; set; }
        [Inject] private PriceHubClient PriceHubClient { get; set; }

        private Dictionary<string, List<string>> MessageConfirmReg = new();
        private bool ShowPreloader { get; set; } = true;
        private bool ShowProductsPreloader { get; set; } = true;

        private List<CategoryDto> _categories = new();
        private PaginatedList<ProductDto> _products = new();
        private string ApiConnection { get; set; }
        private ProductFilters ProductFilters { get; set; } = new ProductFilters();

        public int ProductPageNumber = 0;
        public int MaxProductPageNumber = 0;

        protected override void OnInitialized()
        {
            ApiConnection = Configuration.GetConnectionString("Api");
            _products.List = new();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                ShowPreloader = true;
                PriceHubClient.ResetChangedPriceSingle();
                PriceHubClient.ChangedPrice += ChangedPrice;

                await GetCategories();
                await GetProductStates();
                SetDefaultFilters(_categories);

                if (SearchResult.Length > 0 && SearchResult[0] != 0)
                {
                    await GetSearchProducts();
                    MessageService.ShowInfo("ЗНАЙДЕНО!", $"{SearchResult.Length} - {(SearchResult.Length > 1 ? "товарів" : "товар")}");
                }
                else if (SearchResult.Length > 0 && SearchResult[0] == 0)
                {
                    ShowProductsPreloader = true;
                    MessageService.ShowError("НЕ ЗНАЙДЕНО!", "За вашим запитом нічого не знайдено");
                }
                else
                {
                    await SubmitFilters();
                }

                ShowPreloader = false;
                StateHasChanged();
            }
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
            ShowProductsPreloader = true;

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

            var responce = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetFavoriteProductsWithFilters(new ProductFilterInfo
            { Categories = categories, States = states, PriceStart = ProductFilters.PriceStart, PriceEnd = ProductFilters.PriceEnd }, ProductPageNumber));
            _products = responce.Data;

            if (_products is not null)
            {
                MaxProductPageNumber = _products.CountAll % ProductListConstant.MAXONPAGE == 0 ? (_products.CountAll / ProductListConstant.MAXONPAGE) : (_products.CountAll / ProductListConstant.MAXONPAGE) + 1;
            }

            ShowProductsPreloader = false;
        }

        protected void SetDefaultFilters(List<CategoryDto> categories)
        {
            categories.ForEach(category => ProductFilters.Categories.Add(new Category { Id = category.Id, Name = category.Name, Selected = true }));
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
                {
                    x.Price = changedProduct.Price;
                    MessageService.ShowInfo("Ціна змінилася", $"{x.Title} - {Math.Round(x.Price + x.Price * (decimal)0.02, 2)}");
                }
            });
        }

        private async Task GetSearchProducts()
        {
            ShowProductsPreloader = true;
            _products.List = (await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetSearch(new SearchIdsDto { Ids = SearchResult.ToList() }))).Data;
            ShowProductsPreloader = false;
        }
    }
}