using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Models.Products;

namespace StepEbay.Admin.Api.Common.Services.Products
{
    public interface ICategoryService
    {
        public Task<BoolResult> AddCategory(string categoryName);
        public Task<BoolResult> RemoveCategory(int id);
        public Task<BoolResult> RemoveCategory(string categoryName);
        public Task<BoolResult> UpdateCategory(int id, string newName);
        public Task<BoolResult> UpdateCategory(string oldToken, string newName);
        public Task<List<Category>> GetAllCategotys();
    }
}
