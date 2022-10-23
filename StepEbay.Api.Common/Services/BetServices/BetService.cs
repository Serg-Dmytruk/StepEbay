using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.BetsDbServices;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Models.Bets;
using StepEbay.Data.Models.Products;
using StepEbay.Main.Common.Models.Bet;
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
        private readonly IProductDbService _productDbService;
        

        public BetService(IPurchesDbService purchesDbService,
            IProductDbService productDbService)
        {
            _purchesDbService = purchesDbService;
            _productDbService = productDbService;
        }

        public async Task<ResponseData> PlaceBet(int userId, int productId)
        {
            Product product = _productDbService.Get(productId).Result;

            Purchase purchase = new() { PurchasePrice = product.Price, PoductId = productId, UserId = userId, PurchaseStateId = product.PurchaseTypeId == 1 ? 2 : 1 };//1-sale, 2-auction

            if (product.PurchaseTypeId == 1) //Коли одразу купляємо
            {
                product.IsActive = false;
                await _productDbService.Update(product);
            }
            else //Коли ставимо ставку
            {
                Purchase lastPurchase =_purchesDbService.GetPurchasesByProductId(productId).Result.Last();
                purchase.PurchasePrice = lastPurchase.PurchasePrice * (decimal)1.02;
                product.DateClose = product.DateClose.Value.AddMinutes(10);
                await _productDbService.Update(product);
            }

            await _purchesDbService.Add(purchase);

            return ResponseData.Ok();
        }

        public async Task<List<PurchaseDto>> GetPurchase(int productId)
        {
            return (await _purchesDbService.GetPurchasesByProductId(productId))
                .Select(n=>new PurchaseDto() {
                    Id=n.Id,
                    PoductId=n.PoductId,
                    PurchasePrice=n.PurchasePrice,
                    UserId=n.UserId,
                    PurchaseStateId=n.PurchaseStateId 
                }).ToList();
        }
    }
}