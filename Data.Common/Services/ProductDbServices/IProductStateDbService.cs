using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Products;

namespace StepEbay.Data.Common.Services.ProductDbServices
{
    public interface IProductStateDbService : IDefaultDbService<int, ProductState>
    {
        public Task<bool> AnyStateByName(string name);

        public Task<List<ProductState>> GetAllProducts();
    }
}
