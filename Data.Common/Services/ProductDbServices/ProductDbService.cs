using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Products;
using StepEbay.Main.Common.Models.Product;

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

        public async Task<bool> AnyProductsByState(int stateId)
        {
            return await _context.Products.AnyAsync(p => p.ProductStateId == stateId);
        }

        public async Task<List<Product>> GetProductsByCategory(int categoryId)
        {
            return await _context.Products.Where(p => p.CategoryId == categoryId).ToListAsync();
        }
        private IQueryable<Product> GetFilteredProducts(ProductFilters filter)
        {
            IQueryable<Product> query = _context.Products;

            if (filter.category != 0)
                query = query.Where(p => p.CategoryId == filter.category);

            if (filter.state != 0)
                query = query.Where(p => p.ProductStateId == filter.state);

            if (filter.priceStart != 0)
                query = query.Where(p => p.Price >= filter.priceStart);

            if (filter.priceEnd != 0)
                query = query.Where(p => p.Price <= filter.priceEnd);

            return query;
        }

        public async Task<List<Product>> GetProductList(ProductFilters filter)
        {
            return await GetFilteredProducts(filter).Skip(1).Take(1).ToListAsync();
        }

        public async Task<List<Product>> GetProducts(int page)
        {
            return await _context.Products.Skip(page * 3).Take(3).ToListAsync();
        }

        public async Task<int> GetCount()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<int> GetProductCount(ProductFilters filter)
        {
            return await GetFilteredProducts(filter).CountAsync();
        }
    }
}
