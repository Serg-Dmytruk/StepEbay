using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Products;

namespace StepEbay.Data.Common.Services.ProductDbServices
{
    public interface IProductDescDbService : IDefaultDbService<int, ProductDesc>
    {
        public Task<List<ProductDesc>> GetProductDescByProductId(int productId);
    }
}
