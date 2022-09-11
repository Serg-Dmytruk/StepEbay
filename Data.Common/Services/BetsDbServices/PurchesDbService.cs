using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Bets;

namespace StepEbay.Data.Common.Services.BetsDbServices
{
    public class PurchesDbService : DefaultDbService<int, Purchase>, IPurchesDbService
    {
        private readonly ApplicationDbContext _dbContext;
        public PurchesDbService(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Purchase>> GetPurchaseByUserId(int id)
        {
            return await _dbContext.Purchases.Where(p => p.UserId == id).ToListAsync();
        }
    }
}
