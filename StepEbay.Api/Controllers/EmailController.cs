using Microsoft.AspNetCore.Mvc;
using StepEbay.Main.Api.Common.Services.EmailSenderServices;
using Swashbuckle.AspNetCore.Annotations;

namespace StepEbay.Main.Api.Controllers
{
    [Route("email")]
    public class EmailController : ControllerBase
    {
        private IEmailSenderService _emailSenderService;
        public EmailController(IEmailSenderService emailSenderService)
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
            await _emailSenderService.SendEmail(mail, title, description);
        }

        [HttpPost("end1")]
        public async Task Send1(string mail, string title, string description)
        {
            await _emailSenderService.SendEmail(mail, title, description);
        }

        [HttpPost("end2")]
        public async Task Send2(string mail, string title, string description)
        {
            await _emailSenderService.SendEmail(mail, title, description);
        }
    }
}
