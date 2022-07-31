using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Bets;

namespace StepEbay.Data.Common.Services.BetsDbServices
{
    public class PurchesStateDbService : DefaultDbService<int, PurchaseState>, IPurchesStateDbService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PurchesStateDbService(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> AnyByState(string state)
        {
            return await _applicationDbContext.PurchaseStates.AnyAsync(p => p.State == state);
        }
    }
}
