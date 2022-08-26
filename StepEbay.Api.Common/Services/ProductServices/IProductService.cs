using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Models.Products;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Common.Services.ProductServices
{
    public interface IProductService
    {
        public Task<BoolResult> AddProduct(ProductRequestDto productRequest);
        public Task<ResponseData<List<CategoryResponseDto>>> GetAllCategorys();
        public Task<ResponseData<List<ProductStateResponseDto>>> GetAllStates();


        //hc next
        public Task<List<Product>> GetAllProduct();
    }
}
