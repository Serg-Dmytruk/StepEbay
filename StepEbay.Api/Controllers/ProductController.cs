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
        public async Task<PaginatedList<ProductDto>> GetFilteredProducts([FromBody] ProductFilters filter)
        {
            return await _productService.GetProductList(filter);
        }

        [HttpPost("all")]
        public async Task<PaginatedList<ProductDto>> GetProducts(int page)
        {
            return await _productService.GetProducts(page);
        }

        [HttpPost("categories")]
        public async Task<List<CategoryDto>> GetCategories()
        {
            return await _productService.GetCategoryList();
        }

/*        [HttpPost("filtered")]
        public async Task<PaginatedList<ProductDto>> GetProducts(int[] categoryIds, int minSum, int maxSum, int stateId)
        {
            
        }*/
    }
}
