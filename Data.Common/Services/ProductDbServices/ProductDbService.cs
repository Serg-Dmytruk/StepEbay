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

        public async Task<List<Product>> GetFilteredProducts(ProductFilterInfo info)
        {
            var filteredList = await _context.Products.Where(product => info.Categories.Any(cId => cId == product.CategoryId) 
            && info.States.Any(s => product.ProductStateId == s) && info.PriceStart <= product.Price && info.PriceEnd >= product.Price).ToListAsync();

            return filteredList;
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<int> GetCount()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<List<Product>> GetProductForInfo(List<int> products)
        {
            return await _context.Products.Where(x => products.Contains(x.Id)).ToListAsync();
        }
    }
}
