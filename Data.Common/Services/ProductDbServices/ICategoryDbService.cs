using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Products;

namespace StepEbay.Data.Common.Services.ProductDbServices
{
    public interface ICategoryDbService : IDefaultDbService<int, Category>
    {
        public Task<bool> AnyByName(string name);
        public Task<List<Category>> GetAllCategories();
    }
}
