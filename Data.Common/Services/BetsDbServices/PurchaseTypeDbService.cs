using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Bets;

namespace StepEbay.Data.Common.Services.BetsDbServices
{
    public class PurchaseTypeDbService : DefaultDbService<int, PurchaseType>, IPurchaseTypeDbService
    {
        private readonly ApplicationDbContext _context;
        public PurchaseTypeDbService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> AnyByName(string name)
        {
            return await _context.PurchaseTypes.AnyAsync(p => p.Type == name);
        }

        public async Task<List<PurchaseType>> GetAll()
        {
            return await _context.PurchaseTypes.ToListAsync();
        }
    }
}
