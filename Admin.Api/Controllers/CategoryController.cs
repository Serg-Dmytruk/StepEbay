using Microsoft.AspNetCore.Mvc;
using StepEbay.Admin.Api.Common.Services.Products;
using StepEbay.Admin.Common.Models.Products;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Models.Products;

namespace StepEbay.Admin.Api.Controllers
{
    [Route("product")]
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
        [HttpPut("category/add/{categoryName}")]
        public async Task<BoolResult> AddCategory(string categoryName)
        {
            return await _categoryService.AddCategory(categoryName);
        }

        /// <summary>
        /// Видаляє категорію за ІД
        /// </summary>
        [HttpDelete("category/removeid/{categoryId}")]
        public async Task<BoolResult> RemoveGroup(int categoryId)
        {
            return await _categoryService.RemoveCategory(categoryId);
        }

        /// <summary>
        /// Видаляє категорію за ім'ям
        /// </summary>
        [HttpDelete("category/removename/{categoryName}")]
        public async Task<BoolResult> RemoveGroupToken(string categoryName)
        {
            return await _categoryService.RemoveCategory(categoryName);
        }

        /// <summary>
        /// Обновляє категорію за ІД
        /// </summary>
        [HttpPatch("category/updateid/{categoryId}/{newCategoryName}")]
        public async Task<BoolResult> UpdateGroup(int categoryId, string newCategoryName)
        {
            return await _categoryService.UpdateCategory(categoryId, newCategoryName);
        }

        /// <summary>
        /// Обновляє категорію за ім'ям
        /// </summary>
        [HttpPatch("category/updatename/{oldCategoryName}/{newCategoryName}")]
        public async Task<BoolResult> UpdateGroupToken(string oldCategoryName, string newCategoryName)
        {
            return await _categoryService.UpdateCategory(oldCategoryName, newCategoryName);
        }

        /// <summary>
        /// Повертає усі категорії(DTO)
        /// </summary>
        [HttpPost("category/all")]
        public async Task<List<CategoryResponseDto>> GetAllGroupDto()
        {
            List<Category> l = (await _categoryService.GetAllCategotys());
            List<CategoryResponseDto> result = new List<CategoryResponseDto>();
            foreach (Category category in l)
            {
                result.Add(new CategoryResponseDto() { Id = category.Id, Name = category.Name });
            }
            return result;
        }
    }
}
