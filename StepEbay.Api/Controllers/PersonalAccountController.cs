using Microsoft.AspNetCore.Mvc;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Api.Common.Services.PersonalAccountServices;
using StepEbay.Main.Common.Models.Person;
using System.Security.Claims;

namespace StepEbay.Main.Api.Controllers
{
    [Route("person")]
    public class PersonalAccountController : ControllerBase
    {
        IPersonService _personService { get; set; }
        public PersonalAccountController(IPersonService personService)
        {
            _personService = personService;
        }
        [HttpPost("update/{person}")]
        public async Task<BoolResult> TryUpdate([FromBody] PersonUpdateRequestDto person)
        {
            int id = int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.Name).Value);
            var result = await _personService.TryUpdate(id, person);
            return result;
        }
        [HttpPost("get")]
        public async Task<ResponseData<PersonResponseDto>> GetPersonToUpdateInCabinet()
        {
            int id = int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.Name).Value);
            return await _personService.GetPersonToUpdateInCabinet(id);
        }
    }
}
