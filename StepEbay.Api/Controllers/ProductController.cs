using Microsoft.AspNetCore.Mvc;
using StepEbay.Common.Models.Pagination;
using StepEbay.Main.Api.Common.Services.ProductServices;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Controllers
{
    [Route("product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost("all")]
        public async Task<PaginatedList<ProductDto>> GetProducts(int page, string categoryId)
        {
            return await _productService.GetProducts(page, categoryId);
        }

        [HttpGet("categories")]
        public async Task<List<CategoryDto>> GetCategories()
        {
            return await _productService.GetCategoryList();
        }

        [HttpPost("filtered")]
        public async Task<PaginatedList<ProductDto>> GetFilteredProducts([FromBody] ProductFilterInfo info, int page)
        {
            return await _productService.GetFilteredProducts(info, page);
        }

        [HttpGet("states")]
        public async Task<List<ProductStateDto>> GetProductStates()
        {
            return await _productService.GetProductStates();
        }
    }
}
