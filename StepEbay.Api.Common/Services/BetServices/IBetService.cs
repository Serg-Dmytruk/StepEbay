using StepEbay.Common.Models.RefitModels;

namespace StepEbay.Main.Api.Common.Services.BetServices
{
    public interface IBetService
    {
        Task<ResponseData> PlaceBet(int userId, int productId);
    }
}
