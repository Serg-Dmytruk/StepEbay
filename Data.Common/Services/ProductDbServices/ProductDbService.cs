using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Products;

namespace StepEbay.Data.Common.Services.ProductDbServices
{
    public class ProductDbService : DefaultDbService<int, Product>, IProductDbService
    {
        private readonly ApplicationDbContext _context;
        public ProductDbService(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<bool> AnyProductsByTitle(string productTitle)
        {
            return await _context.Products.AnyAsync(p => p.Title == productTitle);
        }

        public async Task<bool> AnyProductsByCategory(int categoryId)
        {
            return await _context.Products.AnyAsync(p => p.CategoryId == categoryId);
        }

        public async Task<List<Product>> GetProductsByCategory(int categoryId)
        {
            return await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
        }
    }
}
