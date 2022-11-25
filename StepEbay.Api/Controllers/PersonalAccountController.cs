using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Models.Products;
using StepEbay.Main.Api.Common.Services.PersonalAccountServices;
using StepEbay.Main.Common.Models.Bet;
using StepEbay.Main.Common.Models.Person;
using StepEbay.Main.Common.Models.Product;
using System.Security.Claims;

namespace StepEbay.Main.Api.Controllers
{
    [Authorize]
    [Route("person")]
    public class PersonalAccountController : ControllerBase
    {
        IPersonService _personService { get; set; }
        public PersonalAccountController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpPost("update")]
        public async Task<ResponseData> TryUpdate([FromBody] PersonUpdateRequestDto person)
        {
            return await _personService.TryUpdate(int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.Name).Value), person);
        }

        [HttpGet("get")]
        public async Task<PersonResponseDto> GetPersonToUpdateInCabinet()
        {
            return await _personService.GetPersonToUpdateInCabinet(int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.Name).Value));
        }

        [HttpPost("info")]
        public async Task<List<ProductDto>> GetProductsInfo([FromBody] ProductInfoDto productInfos)
        {
            return await _personService.GetProductsInfo(productInfos);
        }

        [HttpPost("favorite/{product}")]
        public async Task<BoolResult> ToggleFavorite(int product)
        {
            int userId = int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.Name).Value);
            return await _personService.ToggleFavorite(product, userId);
        }
        [HttpPost("isfavorite/{product}")]
        public async Task<BoolResult> IsFavorite(int product)
        {
            int userId = int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.Name).Value);
            return await _personService.IsFavorite(product, userId);
        }
    }
}
