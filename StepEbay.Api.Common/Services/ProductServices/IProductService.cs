using StepEbay.Common.Models.Pagination;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Models.Products;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Common.Services.ProductServices
{
    public interface IProductService
    {
        public Task<PaginatedList<ProductDto>> GetFilteredProducts(ProductFilterInfo info, int page);
        public Task<List<ProductStateDto>> GetProductStates();
        public Task<List<CategoryDto>> GetCategoryList();
        public Task<ResponseData> AddProduct(int ownerId, ProductDto productRequest);
        public Task<List<PurchaseTypeResponseDto>> GetAllPurchaseTypes();
        public Task<PaginatedList<ProductDto>> GetPersonalProductList(int id, int page, bool active, bool closed);
    }
}
