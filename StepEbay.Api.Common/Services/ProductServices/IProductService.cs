using StepEbay.Common.Models.Pagination;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Common.Services.ProductServices
{
    public interface IProductService
    {
        Task<PaginatedList<ProductDto>> GetProducts(int page, string categoryId);
        Task<List<CategoryDto>> GetCategoryList();
        Task<PaginatedList<ProductDto>> GetFilteredProducts(ProductFilterInfo info, int page);
        Task<List<ProductStateDto>> GetProductStates();
        public Task<PaginatedList<ProductDto>> GetProductList(ProductFilters filters);
        public Task<PaginatedList<ProductDto>> GetProducts(int page);
        public Task<List<CategoryDto>> GetCategoryList();
        public Task<BoolResult> AddProduct(ProductDto productRequest);
        public Task<ResponseData<List<StateDto>>> GetAllStates();
        public Task<ResponseData<List<PurchaseTypeResponseDto>>> GetAllPurchaseTypes();
    }
}
