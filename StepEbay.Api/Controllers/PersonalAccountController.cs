using Microsoft.AspNetCore.Mvc;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Models.Users;
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
        [HttpPost("update/{passwordconfirm}/{nick}/{email}/{password}/{repassword}/{name}/{adress}")]
        public async Task<ResponseData<BoolResult>> TryUpdate(string passwordconfirm, string nick, string email, string password, string repassword, string name, string adress)
        {
            int id = int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.Name).Value);
            return await _personService.TryUpdate(id,nick, email, password, repassword, name, adress, passwordconfirm);
        }
        [HttpGet("get")]
        public async Task<ResponseData<PersonResponseDto>> GetPersonToUpdateInCabinet()
        {
            int id = int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.Name).Value);
            return await _personService.GetPersonToUpdateInCabinet(id);
        }

        //hardcode next door
        [HttpPost("update/{id}/{passwordconfirm}/{nick}/{email}/{password}/{repassword}/{name}/{adress}")]
        public async Task<ResponseData<BoolResult>> TryUpdateHC(int id, string nick, string email, string password, string repassword, string name, string adress)
        {
            return await _personService.TryUpdate(id, nick, email, password, repassword, name, adress, passwordconfirm);
        }
        [HttpPost("get/all")]
        public ResponseData<List<User>> GetAllHC()
        {
            return _personService.GetAllHC();
        }
    }
}
