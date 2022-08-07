using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Models.Products;

namespace StepEbay.Admin.Api.Common.Services.Products
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDbService _categorys;
        public CategoryService(ICategoryDbService categorys)
        {
            _categorys = categorys;
        }
        public async Task<BoolResult> AddCategory(string categoryName)
        {
            await _categorys.Add(new Category()
            {
                Name = categoryName
            });
            return new BoolResult(true);
        }
        public async Task<BoolResult> RemoveCategory(int id)
        {
            await _categorys.Remove(_categorys.Get(id).Result);
            return new BoolResult(true);
        }
        public async Task<BoolResult> RemoveCategory(string categoryName)
        {
            await _categorys.Remove(_categorys.GetByName(categoryName).Result);
            return new BoolResult(true);
        }
        public async Task<BoolResult> UpdateCategory(int id, string newName)
        {
            await _categorys.Update(new Category() { Id = id, Name = newName });
            return new BoolResult(true);
        }
        public async Task<BoolResult> UpdateCategory(string oldToken, string newName)
        {
            Category _onEditiDeveloperGroup = _categorys.GetByName(oldToken).Result;
            _onEditiDeveloperGroup.Name = newName;
            await _categorys.Update(_onEditiDeveloperGroup);
            return new BoolResult(true);
        }
        public async Task<List<Category>> GetAllCategotys()
        {
            return await _categorys.GetAllCategories();
        }
    }
}
