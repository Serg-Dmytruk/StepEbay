using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("list")]
        public async Task<PaginatedList<ProductDto>> GetFilteredProducts([FromBody] ProductFilters filter)
        {
            return await _productService.GetProductList(filter);
        }

        [HttpPost("add/{product}")]
        public async Task<BoolResult> AddProduct([FromBody] ProductDto product)
        {
            product.OwnerId = int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.Name).Value);
            if (product.Image == null)
                product.Image = "none";

            return await _productService.AddProduct(product);
        }

        [HttpPost("categories")]
        public async Task<List<CategoryDto>> GetCategories()
        {
            return await _productService.GetCategoryList();
        }

        [HttpGet("state")]
        public async Task<ResponseData<List<StateDto>>> GetAllStates()
        {
            return await _productService.GetAllStates();
        }

        [HttpGet("type")]
        public async Task<ResponseData<List<PurchaseTypeResponseDto>>> GetAllPurchaseTypes()
        {
            return await _productService.GetAllPurchaseTypes();
        }

        [HttpGet("all")]
        public async Task<PaginatedList<ProductDto>> GetProducts(int page)
        {
            return await _productService.GetProducts(page);
        }

        /*        [HttpPost("filtered")]
                public async Task<PaginatedList<ProductDto>> GetProducts(int[] categoryIds, int minSum, int maxSum, int stateId)
                {

                }*/
    }
}
