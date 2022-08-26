using Microsoft.AspNetCore.Mvc;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Models.Products;
using StepEbay.Main.Api.Common.Services.ProductServices;
using StepEbay.Main.Common.Models.Person;
using StepEbay.Main.Common.Models.Product;
using System.Security.Claims;

namespace StepEbay.Main.Api.Controllers
{

    [Route("product")]
    public class ProductController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService= productService;
        }

        [HttpPost("add/{product}")]
        public async Task<BoolResult> AddProduct([FromBody] ProductRequestDto product)
        {
            return await _productService.AddProduct(product);
        }

        [HttpGet("category")]
        public async Task<ResponseData<List<CategoryResponseDto>>> GetAllCategorys()
        {
            return await _productService.GetAllCategorys();
        }

        [HttpGet("state")]
        public async Task<ResponseData<List<ProductStateResponseDto>>> GetAllStates()
        {
            return await _productService.GetAllStates();
        }

        //hc next
        [HttpGet("all")]
        public async Task<List<Product>> GetAllProduct()
        {
            return await _productService.GetAllProduct();
        }
    }
}
