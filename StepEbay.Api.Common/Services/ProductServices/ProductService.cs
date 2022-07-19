using StepEbay.Common.Models.Pagination;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Common.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private ProductDbService _productDb;
        public ProductService(ProductDbService productDb)
        {
            _productDb = productDb;
        }
         
        public async Task<PaginatedList<ProductDto>> GetProductList(ProductFilters filters)
        {
            var products = await _productDb.GetProductList(filters);

            return new PaginatedList<ProductDto>
            {
                List = products.Select(x => new ProductDto { Title = x.Title }).ToList(),
                CountAll = await _productDb.GetProductCount(filters)
            };
        }
    }
}
