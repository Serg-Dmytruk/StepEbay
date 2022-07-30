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

        [HttpPost("list")]
        public async Task<PaginatedList<ProductDto>> GetProducts([FromBody] ProductFilters filter)
        {
            return await _productService.GetProductList(filter);
        }

        [HttpPost("categories")]
        public async Task<List<CategoryDto>> GetCategories()
        {
            return await _productService.GetCategoryList();
        }
    }
}
