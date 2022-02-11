using Microsoft.AspNetCore.Mvc;

namespace StepEbay.Admin.Api.Controllers
{
    [Route("exception")]
    public class ExceptionController : ControllerBase
    {
        public ExceptionController()
        {

        }

        /// <summary>
        /// Глобальний обробник помилок
        /// </summary>
        [HttpGet("log")]
        public async Task LogException()
        {

        }
    }
}
