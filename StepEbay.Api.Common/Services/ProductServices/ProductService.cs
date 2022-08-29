using StepEbay.Common.Models.Pagination;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.BetsDbServices;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Models.Bets;
using StepEbay.Data.Models.Products;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Common.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private IProductDbService _productDb;
        private ICategoryDbService _categoryDb;
        private IProductStateDbService _productStateDb;
        private IPurchaseTypeDbService _purchaseTypeDb;

        public ProductService(IProductDbService productDb, ICategoryDbService categoryDb, IProductStateDbService productStateDb, IPurchaseTypeDbService purchaseTypeDb)
        {
            _productDb = productDb;
            _categoryDb = categoryDb;
            _productStateDb = productStateDb;
            _purchaseTypeDb = purchaseTypeDb;
        }

        public async Task<PaginatedList<ProductDto>> GetFilteredProducts(ProductFilterInfo info, int page)
        {
            var products = await _productDb.GetFilteredProducts(info);

            var p = new PaginatedList<ProductDto>
            {
                List = products.Select(x => new ProductDto { Id = x.Id, Image = x.Image, Title = x.Title, Description = x.Description, Price = x.Price, ByNow = x.ByNow, Count = x.Count, CategoryId = x.CategoryId, StateId = x.ProductStateId, OwnerId = x.OwnerId, PurchaseTypeId = x.PurchaseTypeId, DateCreated = x.DateCreated }).Skip(page * 3).Take(3).ToList(),
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

        public async Task<BoolResult> AddProduct(ProductDto productRequest)
        {
            Product product = new Product()
            {
                DateCreated = DateTime.Now,
                DateClose = DateTime.Now.AddDays(2),
                Image = productRequest.Image,
                Title = productRequest.Title,
                Description = productRequest.Description,
                Price = productRequest.Price,
                ByNow = productRequest.ByNow,
                Count = productRequest.Count,
                CategoryId = productRequest.CategoryId,
                ProductStateId = productRequest.StateId,
                PurchaseTypeId = productRequest.PurchaseTypeId,
                OwnerId = productRequest.OwnerId
            };

            await _productDb.Add(product);

            return new BoolResult(true);
        }

        public async Task<ResponseData<List<PurchaseTypeResponseDto>>> GetAllPurchaseTypes()
        {
            List<PurchaseTypeResponseDto> converted = new List<PurchaseTypeResponseDto>();
            List<PurchaseType> list = await _purchaseTypeDb.GetAll();
            foreach (PurchaseType type in list)
                converted.Add(new PurchaseTypeResponseDto() { Id = type.Id, Name = type.Type });

            return new ResponseData<List<PurchaseTypeResponseDto>>() { Data = converted };
        }

        public async Task<List<ProductStateDto>> GetProductStates()
        {
            var productStates = await _productStateDb.GetAll();
            return productStates.Select(x => new ProductStateDto { Id = x.Id, Name = x.Name }).ToList();
        }
    }
}
