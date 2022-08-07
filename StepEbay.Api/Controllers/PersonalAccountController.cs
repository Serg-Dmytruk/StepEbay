using Microsoft.AspNetCore.Mvc;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Api.Common.Services.PersonalAccountServices;

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

        //TODO
        [HttpGet("setble/{nik}")]
        public async Task<ResponseData<BoolResult>> ifSeatebleNukname(string nik)
        {
            //    return new BoolResult(true); /*{ Value = false /*await _personService.ifValidNikname(nik) }*/
            //}
            return null;
        }
    }
}
