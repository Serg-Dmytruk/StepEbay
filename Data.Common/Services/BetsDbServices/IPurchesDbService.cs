using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Bets;

namespace StepEbay.Data.Common.Services.BetsDbServices
{
    public interface IPurchesDbService : IDefaultDbService<int, Purchase>
    {
        public Task<List<Purchase>> GetPurchaseByUserId(int id);
    }
}
