using Microsoft.AspNetCore.Mvc;
using StepEbay.Common.Models.Pagination;
using StepEbay.Main.Api.Common.Services.ProductServices;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Controllers
{
    [Route("/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        [HttpPost("filters")]
        public async Task<PaginatedList<ProductDto>> GetProducts([FromBody] ProductFilters filter)
        {
            return await _productService.GetProductList(filter);
        }
    }
}
