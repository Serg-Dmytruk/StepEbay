using Microsoft.AspNetCore.Mvc;
using StepEbay.Common.Models.Pagination;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Api.Common.Services.ProductServices;
using StepEbay.Main.Common.Models.Product;
using System.Security.Claims;

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

        [HttpPost("add")]
        public async Task<BoolResult> AddProduct([FromBody] ProductDto product)
        {
            return await _productService.AddProduct(int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.Name).Value), product);
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

        [HttpGet("type")]
        public async Task<List<PurchaseTypeResponseDto>> GetAllPurchaseTypes()
        {
            return await _productService.GetAllPurchaseTypes();
        }
    }
}
