using Microsoft.AspNetCore.Mvc;
using StepEbay.Admin.Api.Common.Services.Products;
using StepEbay.Admin.Common.Models.Products;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Models.Products;

namespace StepEbay.Admin.Api.Controllers
{
    [Route("category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /// <summary>
        /// Додає нову категорію
        /// </summary>
        [HttpPut("add/{categoryName}")]
        public async Task<BoolResult> AddCategory(string categoryName)
        {
            return await _categoryService.AddCategory(categoryName);
        }

        /// <summary>
        /// Видаляє категорію за ІД
        /// </summary>
        [HttpDelete("removeid/{categoryId}")]
        public async Task<BoolResult> RemoveGroup(int categoryId)
        {
            return await _categoryService.RemoveCategory(categoryId);
        }

        /// <summary>
        /// Видаляє категорію за ім'ям
        /// </summary>
        [HttpDelete("removename/{categoryName}")]
        public async Task<BoolResult> RemoveCategoryName(string categoryName)
        {
            return await _categoryService.RemoveCategory(categoryName);
        }

        /// <summary>
        /// Обновляє категорію за ІД
        /// </summary>
        [HttpPatch("updateid/{categoryId}/{newCategoryName}")]
        public async Task<BoolResult> UpdateGroup(int categoryId, string newCategoryName)
        {
            return await _categoryService.UpdateCategory(categoryId, newCategoryName);
        }

        /// <summary>
        /// Обновляє категорію за ім'ям
        /// </summary>
        [HttpPatch("updatename/{oldCategoryName}/{newCategoryName}")]
        public async Task<BoolResult> UpdateGroupName(string oldCategoryName, string newCategoryName)
        {
            return await _categoryService.UpdateCategory(oldCategoryName, newCategoryName);
        }

        /// <summary>
        /// Повертає усі категорії(DTO)
        /// </summary>
        [HttpPost("all")]
        public async Task<List<CategoryResponseDto>> GetAllCategoryDto()
        {
            List<Category> l = (await _categoryService.GetAllCategories());
            List<CategoryResponseDto> result = new List<CategoryResponseDto>();
            foreach (Category category in l)
            {
                result.Add(new CategoryResponseDto() { Id = category.Id, Name = category.Name });
            }
            return result;
        }
    }
}
