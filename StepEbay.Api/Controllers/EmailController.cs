using Microsoft.AspNetCore.Mvc;
using StepEbay.Main.Api.Common.Services.EmailSenderServices;

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
        [HttpPost("news")]
        public async Task SendNews(string title, string description)
        {
            string mail = "";//get mail from session
            await _emailSenderService.SendEmail(mail, title, description);//TODO: content of mail must be changed
        }

        /// <summary>
        /// Відправлення інформації про ставку з стандартного адресу
        /// </summary>
        [HttpPost("bet")]
        public async Task SendBetMessage()
        {
            string mail = "";//get mail from session
            await _emailSenderService.SendEmail(mail, title, description);//TODO: content of mail must be changed
        }

        /// <summary>
        /// Відправлення "підтвердження реєстрації" з стандартного адресу
        /// </summary>
        [HttpPost("SendRegistrationConfirm")]
        public async Task SendRegistrationConfirm()
        {
            string mail = "";//get mail from session
            await _emailSenderService.SendEmail(mail, title, description);//TODO: content of mail must be changed
        }
    }
}
