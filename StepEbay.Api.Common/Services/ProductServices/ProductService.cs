using StepEbay.Common.Models.Pagination;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.BetsDbServices;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Models.Products;
using StepEbay.Main.Api.Common.Services.DataValidationServices;
using StepEbay.Main.Common.Models.Product;
using StepEbay.Common.Constans;

namespace StepEbay.Main.Api.Common.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IProductDbService _productDb;
        private readonly ICategoryDbService _categoryDb;
        private readonly IProductStateDbService _productStateDb;
        private readonly IPurchaseTypeDbService _purchaseTypeDb;
        private readonly IPurchesDbService _purchaseDb;

        public ProductService(IProductDbService productDb,
            ICategoryDbService categoryDb,
            IProductStateDbService productStateDb,
            IPurchaseTypeDbService purchaseTypeDb,
            IPurchesDbService purchaseDb)
        {
            _productDb = productDb;
            _categoryDb = categoryDb;
            _productStateDb = productStateDb;
            _purchaseTypeDb = purchaseTypeDb;
            _purchaseDb = purchaseDb;
        }
        public async Task<List<ProductDto>> GetSearch(SearchIdsDto products)
        {
            return (await _productDb.GetProductByIds(products)).Select(x => new ProductDto 
            { 
                Id = x.Id, 
                Image = x.Image, 
                Title = x.Title, 
                Description = x.Description, 
                Price = x.Price, 
                CategoryId = x.CategoryId, 
                StateId = x.ProductStateId, 
                OwnerId = x.OwnerId, 
                PurchaseTypeId = x.PurchaseTypeId,
                DateCreated = x.DateCreated,
                AdditionalInfo = x.AditionalInfo,
                Rate = x.Rate
            }).ToList();
        }

        public async Task<SearchIdsDto> Search(string product)
        {
            return new SearchIdsDto
            {
                Ids = await _productDb.GetSearchIds(product)
            };
        }

        public async Task<PaginatedList<ProductDto>> GetFilteredProducts(ProductFilterInfo info, int page)
        {
            var products = await _productDb.GetFilteredProducts(info);

            return new PaginatedList<ProductDto>
            {
                List = products.Select(x => new ProductDto { 
                    Id = x.Id,
                    Image = x.Image,
                    Title = x.Title,
                    Description = x.Description,
                    Price = x.Price,
                    CategoryId = x.CategoryId,
                    StateId = x.ProductStateId,
                    OwnerId = x.OwnerId,
                    PurchaseTypeId = x.PurchaseTypeId,
                    DateCreated = x.DateCreated,
                    AdditionalInfo = x.AditionalInfo,
                    Rate=x.Rate
                }).Skip(page * ProductListConstant.MAXONPAGE).Take(ProductListConstant.MAXONPAGE).ToList(),
                CountAll = products.Count()
            };
        }

        public async Task<List<CategoryDto>> GetCategoryList()
        {
            return (await _categoryDb.GetAll()).Select(x => new CategoryDto { Id = x.Id, Name = x.Name }).ToList();
        }

        public async Task<ResponseData> AddProduct(int ownerId, ProductDto productRequest)
        {
            productRequest.OwnerId = ownerId;

            ProductValidator validator = new();
            validator.Validate(productRequest);
            var result = validator.Validate(productRequest);

            if (!result.IsValid)
                return ResponseData.Fail("Product", result.Errors.First().ErrorMessage);

            var product = new Product()
            {
                DateCreated = DateTime.UtcNow,
                DateClose = DateTime.UtcNow.AddMinutes(10),
                Image = productRequest.Image,
                Title = productRequest.Title,
                Description = productRequest.Description,
                Price = productRequest.Price,
                CategoryId = productRequest.CategoryId,
                ProductStateId = productRequest.StateId,
                PurchaseTypeId = productRequest.PurchaseTypeId,
                OwnerId = productRequest.OwnerId,
                Rate = productRequest.Rate,
                AditionalInfo=productRequest.AdditionalInfo
            };

            await _productDb.Add(product);

            return ResponseData.Ok();
        }

        public async Task<List<PurchaseTypeResponseDto>> GetAllPurchaseTypes()
        {
            return (await _purchaseTypeDb.GetAll()).Select(x => new PurchaseTypeResponseDto { Id = x.Id, Name = x.Type }).ToList();
        }

        public async Task<List<ProductStateDto>> GetProductStates()
        {
            var productStates = await _productStateDb.GetAll();
            return productStates.Select(x => new ProductStateDto { Id = x.Id, Name = x.Name }).ToList();
        }

        public async Task<PaginatedList<ProductDto>> GetPersonalProductList(int id, int page, bool active, bool closed)
        {
            var purchases = await _purchaseDb.GetPurchaseByUserId(id);
            var products = await _productDb.GetProducts();
            var productList = products.Where(p => purchases.Any(purch => purch.PoductId == p.Id)).ToList();

            if (active is true && closed is false)
                productList = productList.Where(p => p.DateClose > DateTime.UtcNow).ToList();
            if (active is false && closed is true)
                productList = productList.Where(p => p.DateClose < DateTime.UtcNow).ToList();


            return new PaginatedList<ProductDto>
            {
                List = productList.Select(x => new ProductDto { 
                    Id = x.Id,
                    Image = x.Image,
                    Title = x.Title,
                    Description = x.Description,
                    Price = x.Price,
                    CategoryId = x.CategoryId,
                    StateId = x.ProductStateId,
                    OwnerId = x.OwnerId,
                    PurchaseTypeId = x.PurchaseTypeId,
                    DateCreated = x.DateCreated,
                    DateClosed = (DateTime)x.DateClose,
                    AdditionalInfo=x.AditionalInfo,
                    Rate=x.Rate
                }).Skip(page * 6).Take(6).ToList(),
                CountAll = productList.Count()
            };
        }

        public async Task<ResponseData<ProductDto>> GetProduct(int id)
        {
            
            Product productDb = await _productDb.Get(id);

            if (productDb == null)
                return ResponseData<ProductDto>.Fail("product", "Продукта нема");
       
            return new ResponseData<ProductDto>() { Data = new ProductDto() { Id = productDb.Id, Image = productDb.Image, Title = productDb.Title, Description = productDb.Description, Price = productDb.Price, CategoryId = productDb.CategoryId, StateId = productDb.ProductStateId, OwnerId = productDb.OwnerId, PurchaseTypeId = productDb.PurchaseTypeId, DateCreated = productDb.DateCreated, DateClosed = (DateTime)productDb.DateClose } };
        }
    }
}