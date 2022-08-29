using StepEbay.Common.Models.Pagination;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Common.Services.ProductServices
{
    public interface IProductService
    {
        Task<PaginatedList<ProductDto>> GetProducts(int page, string categoryId);
        Task<List<CategoryDto>> GetCategoryList();
        Task<PaginatedList<ProductDto>> GetFilteredProducts(ProductFilterInfo info, int page);
        Task<List<ProductStateDto>> GetProductStates();
    }
}
