using StepEbay.Common.Models.Pagination;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Common.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private IProductDbService _productDb;
        private ICategoryDbService _categoryDb;
        private IProductStateDbService _productStateDb;
        public ProductService(IProductDbService productDb, ICategoryDbService categoryDb, IProductStateDbService productStateDb)
        {
            _productDb = productDb;
            _categoryDb = categoryDb;
            _productStateDb = productStateDb;
        }
         
        public async Task<PaginatedList<ProductDto>> GetProducts(int page, string categoryId)
        {
            var products = await _productDb.GetProducts();

            if(categoryId == "0")
            {
                return new PaginatedList<ProductDto>
                {
                    List = products.Select(x => new ProductDto { Id = x.Id, Title = x.Title }).Skip(page * 3).Take(3).ToList(),
                    CountAll = await _productDb.GetCount()
                };
            }
            else
            {
                return new PaginatedList<ProductDto>
                {
                    List = products.Where(p => p.CategoryId.ToString() == categoryId).ToList().Select(x => new ProductDto { Id = x.Id, Title = x.Title }).Skip(page * 3).Take(3).ToList(),
                    CountAll = await _productDb.GetCount()
                };
            }
        }

        public async Task<PaginatedList<ProductDto>> GetFilteredProducts(ProductFilterInfo info, int page)
        {
            var products = await _productDb.GetFilteredProducts(info);

            var p = new PaginatedList<ProductDto>
            {
                List = products.Select(x => new ProductDto { Id = x.Id, Title = x.Title }).Skip(page * 3).Take(3).ToList(),
                CountAll = products.Count()
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

        public async Task<List<ProductStateDto>> GetProductStates()
        {
            var productStates = await _productStateDb.GetAll();
            return productStates.Select(x => new ProductStateDto { Id = x.Id, Name = x.Name, Selected = true }).ToList();
        }
    }
}
