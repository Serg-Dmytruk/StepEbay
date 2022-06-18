using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Products;

namespace StepEbay.Data.Common.Services.ProductDbServices
{
    public interface IProductStateDbService : IDefaultDbService<byte, ProductState>
    {
        public Task<bool> AnyStateByName(string name);
    }
}
