using Microsoft.AspNetCore.Mvc;
using StepEbay.Admin.Api.Services.Telegram;
using StepEbay.Data.Common.Services.TelegramDbServices;

namespace StepEbay.Admin.Api.Controllers
{
    [Route("exception")]
    public class ExceptionController : ControllerBase
    {
        private readonly ITelegramService _serviceTg;

        public ExceptionController(ITelegramService serviceTg)
        {
            _serviceTg = serviceTg;
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
            await _serviceTg.SendErrorMessage(exeptionMessage);
        }
    }
}
