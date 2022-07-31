using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Bets;

namespace StepEbay.Data.Common.Services.BetsDbServices
{
    public interface IPurchaseTypeDbService : IDefaultDbService<int, PurchaseType>
    {
        public Task<bool> AnyByName(string name);
        public Task<List<PurchaseType>> GetAll();
    }
}
