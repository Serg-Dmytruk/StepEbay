using StepEbay.Common.Models.Pagination;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.BetsDbServices;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Models.Products;
using StepEbay.Main.Api.Common.Services.DataValidationServices;
using StepEbay.Main.Common.Models.Product;
using StepEbay.Common.Constans;
using StepEbay.Data.Common.Services.UserDbServices;

namespace StepEbay.Main.Api.Common.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly IProductDbService _productDb;
        private readonly IProductDescDbService _productDescDb;
        private readonly ICategoryDbService _categoryDb;
        private readonly IProductStateDbService _productStateDb;
        private readonly IPurchaseTypeDbService _purchaseTypeDb;
        private readonly IPurchesDbService _purchaseDb;
        IFavoriteDbService _favoriteDbService;

        public ProductService(IProductDbService productDb,
            IProductDescDbService productDescDb,
            ICategoryDbService categoryDb,
            IProductStateDbService productStateDb,
            IPurchaseTypeDbService purchaseTypeDb,
            IPurchesDbService purchaseDb,
            IFavoriteDbService favoriteDbService)
        {
            _productDb = productDb;
            _productDescDb = productDescDb;
            _categoryDb = categoryDb;
            _productStateDb = productStateDb;
            _purchaseTypeDb = purchaseTypeDb;
            _purchaseDb = purchaseDb;
            _favoriteDbService = favoriteDbService;
        }
        public async Task<List<ProductDto>> GetSearch(SearchIdsDto products)
        {
            return (await _productDb.GetProductByIds(products)).Select(x => new ProductDto 
            { 
                Id = x.Id,
                Image1 = x.Image1,
                Image2 = x.Image2,
                Image3 = x.Image3,
                Title = x.Title, 
                Description = x.Description, 
                Price = x.Price, 
                CategoryId = x.CategoryId, 
                StateId = x.ProductStateId, 
                OwnerId = x.OwnerId, 
                PurchaseTypeId = x.PurchaseTypeId,
                DateCreated = x.DateCreated,
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
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3,
                    Title = x.Title,
                    Description = x.Description,
                    Price = x.Price,
                    CategoryId = x.CategoryId,
                    StateId = x.ProductStateId,
                    OwnerId = x.OwnerId,
                    PurchaseTypeId = x.PurchaseTypeId,
                    DateCreated = x.DateCreated,
                    Rate=x.Rate
                }).Skip(page * ProductListConstant.MAXONPAGE).Take(ProductListConstant.MAXONPAGE).ToList(),
                CountAll = products.Count()
            };
        }

        public async Task<PaginatedList<ProductDto>> GetFilteredFavoriteProducts(ProductFilterInfo info, int page, int userId)
        {
            var products = await _productDb.GetFilteredProducts(info);

            var list = products.Select(x => new ProductDto
            {
                Id = x.Id,
                Image1 = x.Image1,
                Image2 = x.Image2,
                Image3 = x.Image3,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                CategoryId = x.CategoryId,
                StateId = x.ProductStateId,
                OwnerId = x.OwnerId,
                PurchaseTypeId = x.PurchaseTypeId,
                DateCreated = x.DateCreated,
                Rate = x.Rate
            }).ToList();

            var fav= await _favoriteDbService.GetAllFavorite(userId);

            int count = list.Count;
            for (int i=0;i<count;i++)
            {
                if (!fav.Any(n => n.ProductId == list[i].Id))
                {
                    list.RemoveAt(i);
                    i--;
                    count--;
                }
            }

            list.Skip(page * ProductListConstant.MAXONPAGE).Take(ProductListConstant.MAXONPAGE).ToList();

            return new PaginatedList<ProductDto>
            {
                List = list,
                CountAll = list.Count()
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
                Image1 = productRequest.Image1,
                Image2 = productRequest.Image2,
                Image3 = productRequest.Image3,
                Title = productRequest.Title,
                Description = productRequest.Description,
                Price = productRequest.Price,
                CategoryId = productRequest.CategoryId,
                ProductStateId = productRequest.StateId,
                PurchaseTypeId = productRequest.PurchaseTypeId,
                OwnerId = productRequest.OwnerId,
                Rate = productRequest.Rate
            };

            Product endProduct=await _productDb.Add(product);
            await _productDescDb.AddRange(productRequest.ProductDescs.Select(n => new ProductDesc() { ProductId = product.Id, Name = n.Key, About = n.Value }));

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
                    Image1 = x.Image1,
                    Image2 = x.Image2,
                    Image3 = x.Image3,
                    Title = x.Title,
                    Description = x.Description,
                    Price = x.Price,
                    CategoryId = x.CategoryId,
                    StateId = x.ProductStateId,
                    OwnerId = x.OwnerId,
                    PurchaseTypeId = x.PurchaseTypeId,
                    DateCreated = x.DateCreated,
                    DateClosed = (DateTime)x.DateClose,
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

            var list = await _productDescDb.GetProductDescByProductId(productDb.Id);
            var dictionary=new Dictionary<string, string>();
            foreach (var item in list)
            {
                try {
                    dictionary.Add(item.Name, item.About);
                }
                catch {
                    dictionary.Add(item.Name+ '.', item.About);
                }
            }
            return new ResponseData<ProductDto>() { Data = new ProductDto()
            {
                Id = productDb.Id,
                Image1 = productDb.Image1,
                Image2 = productDb.Image2,
                Image3 = productDb.Image3,
                Title = productDb.Title,
                Description = productDb.Description,
                Price = productDb.Price,
                CategoryId = productDb.CategoryId,
                StateId = productDb.ProductStateId,
                OwnerId = productDb.OwnerId,
                PurchaseTypeId = productDb.PurchaseTypeId,
                DateCreated = productDb.DateCreated,
                DateClosed = (DateTime)productDb.DateClose,
                Rate = productDb.Rate,
                ProductDescs = dictionary
            } };
        }
        public async Task<BoolResult> ToggleFavorite(int productId, int userId)
        {
            return new BoolResult(await _favoriteDbService.ToggleFavorite(productId, userId));
        }

        public async Task<BoolResult> IsFavorite(int productId, int userId)
        {
            return new BoolResult(await _favoriteDbService.IsFavorite(productId, userId));
        }
    }
}