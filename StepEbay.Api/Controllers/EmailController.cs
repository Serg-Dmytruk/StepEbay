using Microsoft.AspNetCore.Mvc;
using StepEbay.Main.Api.Common.Services.EmailSenderServices;
using Swashbuckle.AspNetCore.Annotations;

namespace StepEbay.Main.Api.Controllers
{
    [Route("email")]
    public class EmailController: ControllerBase
    {
        private IEmSenderService _emailSenderService;
        public EmailController(IEmSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
        }

        /// <summary>
        /// Відправлення емейлу з стандартного адресу
        /// </summary>
        [HttpPost]
        [SwaggerResponse(200)]
        public async Task Send(string mail, string title, string description)
        {
            await _emailSenderService.SendEmail(mail,title, description);
        }
    }
}
