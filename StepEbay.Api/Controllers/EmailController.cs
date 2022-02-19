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
        /// Відправлення новин з стандартного адресу
        /// </summary>
        [HttpPost("SendNews")]
        public async Task SendNews(string mail, string title, string description)
        {
            await _emailSenderService.SendEmail(mail, title, description);//TODO: content of mail must be changed
        }

        /// <summary>
        /// Відправлення інформації про ставку з стандартного адресу
        /// </summary>
        [HttpPost("SendBetMessage")]
        public async Task SendBetMessage(string mail, string title, string description)
        {
            await _emailSenderService.SendEmail(mail, title, description);//TODO: content of mail must be changed
        }

        /// <summary>
        /// Відправлення "підтвердження реєстрації" з стандартного адресу
        /// </summary>
        [HttpPost("SendRegistrationConfirm")]
        public async Task SendRegistrationConfirm(string mail, string title, string description)
        {
            await _emailSenderService.SendEmail(mail, title, description);//TODO: content of mail must be changed
        }
    }
}
