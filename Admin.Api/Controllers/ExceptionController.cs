using Microsoft.AspNetCore.Mvc;
using StepEbay.Admin.Api.Services.Telegram;
using StepEbay.Data.Common.Services.TelegramDbServices;

namespace StepEbay.Admin.Api.Controllers
{
    [Route("exception")]
    public class ExceptionController : ControllerBase
    {
        private readonly ITelegramService _serviceTg;
        private readonly IDeveloperGroupDbService _groups;
        public ExceptionController(ITelegramService serviceTg, IDeveloperGroupDbService groups)
        {
            _serviceTg = serviceTg;
            _groups = groups;
        }

        /// <summary>
        /// Глобальний обробник помилок
        /// </summary>
        [HttpGet("log")]
        public async Task LogException()
        {

        }
        /// <summary>
         /// This will send error message through telegram bot
         /// </summary>
        [HttpPost("log/tg")]
        public async Task TgSendExeptionError(string exeptionMessage)
        {
            await _serviceTg.SendErrorMessage(exeptionMessage, _groups);
        }
    }
}
