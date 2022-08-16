using StepEbay.Common.Models.Pagination;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Common.Services.ProductServices
{
    public interface IProductService
    {
        Task<PaginatedList<ProductDto>> GetProductList(ProductFilters filters);
        Task<PaginatedList<ProductDto>> GetProducts(int page);
        Task<List<CategoryDto>> GetCategoryList();
    }
}
