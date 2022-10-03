using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.BetsDbServices;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Models.Bets;
using StepEbay.Data.Models.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace StepEbay.Main.Api.Common.Services.BetServices
{
    public class BetService : IBetService
    {
        private readonly IPurchesDbService _purchesDbService;
        private readonly IProductDbService _productDb;

        public async Task<ResponseData> PlaceBet(int userId, int productId)
        {
            Product product = _productDb.Get(productId).Result;

            if (product.PurchaseTypeId == 1)
            {
                product.IsActive = false;
                await _productDb.Update(product);
            }

            Purchase purchase = new() { PoductId = productId, UserId = userId, PurchaseStateId = product.PurchaseTypeId==1?2:1 };
            await _purchesDbService.Add(purchase);

            return ResponseData.Ok();
        }
    }
}