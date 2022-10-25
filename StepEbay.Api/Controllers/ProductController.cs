using Microsoft.AspNetCore.Mvc;
using StepEbay.Common.Models.Pagination;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Models.Users;
using StepEbay.Main.Api.Common.Services.BetServices;
using StepEbay.Main.Api.Common.Services.ProductServices;
using StepEbay.Main.Common.Models.Bet;
using StepEbay.Main.Common.Models.Product;
using System.Security.Claims;

namespace StepEbay.Main.Api.Controllers
{
    [Route("product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IBetService _betService;

        public ProductController(IProductService productService, IBetService betService)
        {
            _productService = productService;
            _betService = betService;
        }

        [HttpPost("add")]
        public async Task<ResponseData> AddProduct([FromBody] ProductDto product)
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

        [HttpGet("personal/products")]
        public async Task<PaginatedList<ProductDto>> GetPersonalProducts(int page, bool active, bool closed)
        {
            return await _productService.GetPersonalProductList(int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.Name).Value), page, active, closed);
        }

        [HttpGet("{id}")]
        public async Task<ResponseData<ProductDto>> GetProduct(int id)
        {
            return await _productService.GetProduct(id);
        }
        [HttpPost("{id}/bets")]
        public async Task<ResponseData<List<PurchaseDto>>> GetBetsProduct(int id)
        {
            return new ResponseData<List<PurchaseDto>>() { Data= await _betService.GetPurchase(id) } ;
        }
    }
}
