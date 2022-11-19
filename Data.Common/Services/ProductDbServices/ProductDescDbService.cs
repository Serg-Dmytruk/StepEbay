using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Products;

namespace StepEbay.Data.Common.Services.ProductDbServices
{
    public class ProductDescDbService : DefaultDbService<int, ProductDesc>, IProductDescDbService
    {
        private readonly ApplicationDbContext _context;
        public ProductDescDbService(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<List<ProductDesc>> GetProductDescByProductId(int productId)
        {
            return await _context.ProductDesc.Where(n=> n.ProductId==productId).ToListAsync();
        }
    }
}
