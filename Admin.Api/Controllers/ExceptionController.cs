using Microsoft.AspNetCore.Mvc;
using StepEbay.Admin.Api.Common.Models;
using StepEbay.Admin.Api.Services.Telegram;
using System.Text.Json;

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
        [HttpPost("log")]
        public async Task LogException()
        {
            string projectName = HttpContext.Request?.Headers["name"];
            var reader = new StreamReader(HttpContext.Request.Body);
            var body = await reader.ReadToEndAsync();
            reader.Close();
            var exceptionInfo = JsonSerializer.Deserialize<List<Event>>(body);
            await _serviceTg.SendErrorMessage(exceptionInfo, projectName);
        }
    }
}
