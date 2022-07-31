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
    }
}
