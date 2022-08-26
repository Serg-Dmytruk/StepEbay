using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Models.Products;
using StepEbay.Main.Common.Models.Auth;
using StepEbay.Main.Common.Models.Person;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Common.Services.ProductServices
{
    public class ProductService: IProductService
    {
        private readonly IProductDbService _productDbService;
        private readonly ICategoryDbService _categoryDbService;
        private readonly IProductStateDbService _productStateDbService;
        public ProductService(IProductDbService productDbService, ICategoryDbService categoryDbService, IProductStateDbService productStateDbService)
        {
            _productDbService = productDbService;
            _categoryDbService = categoryDbService;
            _productStateDbService = productStateDbService;
        }

        public async Task<BoolResult> AddProduct(ProductRequestDto productRequest)
        {
            Product product=new Product() { DateCreated=productRequest.DateCreated, Image=productRequest.Image, Title=productRequest.Title, Description=productRequest.Description, Price=productRequest.Price, ByNow=productRequest.ByNow, CategoryId=productRequest.IdCategory, ProductStateId=productRequest.IdProductState};
            product.ProductState = await _productStateDbService.Get(product.ProductStateId);
            product.Category = await _categoryDbService.Get(product.CategoryId);
            Product result=await _productDbService.Add(product);

            return new BoolResult(true);
        }

        public async Task<ResponseData<List<CategoryResponseDto>>> GetAllCategorys()
        {
            List<CategoryResponseDto> converted = new List<CategoryResponseDto>();
            List<Category> list = await _categoryDbService.GetAllCategories();
            foreach (Category category in list)
                converted.Add(new CategoryResponseDto() { Id = category.Id, Name = category.Name });
            
            return new ResponseData<List<CategoryResponseDto>>() { Data = converted };
        }

        public async Task<ResponseData<List<ProductStateResponseDto>>> GetAllStates()
        {
            List<ProductStateResponseDto> converted = new List<ProductStateResponseDto>();
            List<ProductState> list = await _productStateDbService.GetAllProducts();
            foreach (ProductState state in list)
                converted.Add(new ProductStateResponseDto() { Id = state.Id, Name = state.Name });

            return new ResponseData<List<ProductStateResponseDto>>() { Data = converted };
        }

        //hc next
        public async Task<List<Product>> GetAllProduct()
        {
            return await _productDbService.List();
        }
    }
}
