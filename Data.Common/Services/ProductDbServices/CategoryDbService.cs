using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Products;

namespace StepEbay.Data.Common.Services.ProductDbServices
{
    public class CategoryDbService : DefaultDbService<int, Category>, ICategoryDbService
    {
        private readonly ApplicationDbContext _context;
        public CategoryDbService(ApplicationDbContext context)
            :base(context)
        {
            _context = context;
        }
        public async Task<bool> AnyByName(string name)
        {
            return await _context.Categories.AnyAsync(c => c.Name == name);
        }

        public async Task<List<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
