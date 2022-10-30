using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Common.Models.Bet;

namespace StepEbay.Main.Api.Common.Services.BetServices
{
    public interface IBetService
    {
        Task<ResponseData> PlaceBet(int userId, int productId);
        Task<List<PurchaseDto>> GetPurchase(int productId);
    }
}
