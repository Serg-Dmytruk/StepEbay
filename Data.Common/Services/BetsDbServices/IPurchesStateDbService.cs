using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Bets;

namespace StepEbay.Data.Common.Services.BetsDbServices
{
    public interface IPurchesStateDbService : IDefaultDbService<int, PurchaseState>
    {
        Task<bool> AnyByState(string state);
    }
}
