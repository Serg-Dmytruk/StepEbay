using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Products;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Data.Common.Services.ProductDbServices
{
    public interface IProductDbService : IDefaultDbService<int, Product> 
    {
        Task<List<Product>> GetProductsByCategory(int categoryId);

        Task<bool> AnyProductsByCategory(int categoryId);

        Task<bool> AnyProductsByState(int stateId);

        Task<bool> AnyProductsByTitle(string productTitle);

        Task<List<Product>> GetProductList(ProductFilters filter);

        Task<int> GetProductCount(ProductFilters filter);
    }
}
