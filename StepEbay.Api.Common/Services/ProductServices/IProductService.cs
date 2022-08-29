using StepEbay.Common.Models.Pagination;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Common.Services.ProductServices
{
    public interface IProductService
    {
        public Task<PaginatedList<ProductDto>> GetFilteredProducts(ProductFilterInfo info, int page);
        public Task<List<ProductStateDto>> GetProductStates();
        public Task<List<CategoryDto>> GetCategoryList();
        public Task<BoolResult> AddProduct(int ownerId, ProductDto productRequest);
        public Task<List<PurchaseTypeResponseDto>> GetAllPurchaseTypes();
    }
}
