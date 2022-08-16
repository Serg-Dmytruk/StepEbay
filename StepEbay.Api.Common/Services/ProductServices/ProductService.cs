using StepEbay.Common.Models.Pagination;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Common.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private IProductDbService _productDb;
        private ICategoryDbService _categoryDb;
        public ProductService(IProductDbService productDb, ICategoryDbService categoryDb)
        {
            _productDb = productDb;
            _categoryDb = categoryDb;
        }
         
        public async Task<PaginatedList<ProductDto>> GetProductList(ProductFilters filters)
        {
            var products = await _productDb.GetProductList(filters);

            return new PaginatedList<ProductDto>
            {
                List = products.Select(x => new ProductDto { Id = x.Id, Title = x.Title }).ToList(),
                CountAll = await _productDb.GetProductCount(filters)
            };
        }

        public async Task<PaginatedList<ProductDto>> GetProducts(int page)
        {
            var products = await _productDb.GetProducts(page);

            var p = new PaginatedList<ProductDto>
            {
                List = products.Select(x => new ProductDto { Id = x.Id, Title = x.Title }).ToList(),
                CountAll = await _productDb.GetCount()
            };

            return p;
        }

        public async Task<List<CategoryDto>> GetCategoryList()
        {
            var categories = await _categoryDb.GetAll();
            var listCategories = new List<CategoryDto>();
            
            foreach (var category in categories)
            {
                listCategories.Add(new CategoryDto { Id = category.Id, Name = category.Name });
            }

            return listCategories;
        }
    }
}
