using Microsoft.AspNetCore.Mvc;
using StepEbay.Admin.Api.Services.Telegram;
using StepEbay.Data.Common.Services.TelegramDbServices;

namespace StepEbay.Admin.Api.Controllers
{
    [Route("error")]
    public class ErrorController : Controller
    {
        private readonly ITelegramService _serviceTg;
        private readonly IDeveloperGroupDbService _groups;
        public ErrorController(ITelegramService serviceTg, IDeveloperGroupDbService groups)
        {
            _serviceTg = serviceTg;
            _groups= groups;
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpPost("log")]
        public async Task AceptError(string errorMessage)
        {
            await _serviceTg.SendErrorMessage(errorMessage, _groups);
            
        }
        [HttpPost("group")]
        public async Task AddGroup(string groupTg)
        {
            await _serviceTg.SaveGroup(groupTg, _groups);
        }
    }
}
