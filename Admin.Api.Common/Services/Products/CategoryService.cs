using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Models.Products;

namespace StepEbay.Admin.Api.Common.Services.Products
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDbService _categories;
        public CategoryService(ICategoryDbService categorys)
        {
            _categories = categorys;
        }
        public async Task<BoolResult> AddCategory(string categoryName)
        {
            await _categories.Add(new Category()
            {
                Name = categoryName
            });
            return new BoolResult(true);
        }
        public async Task<BoolResult> RemoveCategory(int id)
        {
            await _categories.Remove(_categories.Get(id).Result);
            return new BoolResult(true);
        }
        public async Task<BoolResult> RemoveCategory(string categoryName)
        {
            await _categories.Remove(_categories.GetByName(categoryName).Result);
            return new BoolResult(true);
        }
        public async Task<BoolResult> UpdateCategory(int id, string newName)
        {
            await _categories.Update(new Category() { Id = id, Name = newName });
            return new BoolResult(true);
        }
        public async Task<BoolResult> UpdateCategory(string oldToken, string newName)
        {
            Category _onEditiDeveloperGroup = _categories.GetByName(oldToken).Result;
            _onEditiDeveloperGroup.Name = newName;
            await _categories.Update(_onEditiDeveloperGroup);
            return new BoolResult(true);
        }
        public async Task<List<Category>> GetAllCategories()
        {
            return await _categories.GetAll();
        }
    }
}
