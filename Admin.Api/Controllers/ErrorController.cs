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
            _groups = groups;
        }

        /// <summary>
        /// This will send error message through telegram bot
        /// </summary>
        [HttpPost("log")]
        public async Task AceptError(string errorMessage)
        {
            await _serviceTg.SendErrorMessage(errorMessage, _groups);
        }

        /// <summary>
        /// Use it for registrate groups where bot will send messages
        /// </summary>
        [HttpPost("group")]
        public async Task AddGroup(string groupTg)
        {
            await _serviceTg.SaveGroup(groupTg, _groups);
        }
    }
}
