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

            Purchase purchase = new() { PurchasePrice= product.Price, PoductId = productId, UserId = userId, PurchaseStateId = product.PurchaseTypeId==1?2:1 };//1-sale, 2-auction
            await _purchesDbService.Add(purchase);

            if (product.PurchaseTypeId == 1) //Коли одразу купляємо
            {
                product.IsActive = false;
                await _productDb.Update(product);
            }
            else //Коли ставимо ставку
            {
                product.Price = product.Price * (decimal)1.05;
                await _productDb.Update(product);
            }

            return ResponseData.Ok();
        }
    }
}