﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Models.Products;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Product;
using StepEbay.PushMessage.Services;
using System;
using System.Net;

namespace StepEbay.Main.Client.Base.Pages.Products
{
    [Route("/product/{id}")]
    public partial class ProductPage
    {
        [Parameter] public string Id { get; set; }
        [Inject] private IApiService _apiService { get; set; }
        [Inject] IMessageService _messageService { get; set; }

        ProductDto product { get; set; }

        public ProductPage()
        {
            product = new();
        }

        protected override async Task OnInitializedAsync()
        {
            ResponseData<ProductDto> result = await _apiService.ExecuteRequest(() => _apiService.ApiMethods.GetProduct(int.Parse(Id)));

            if (result.StatusCode != HttpStatusCode.OK)
                _messageService.ShowError("Помилка", result.Errors.First().Value.First());

            product = result.Data;

            StateHasChanged();
        }
    }
}