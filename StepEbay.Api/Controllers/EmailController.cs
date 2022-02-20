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
            await _emailSenderService.SendNews(title, description);//TODO: content of mail must be changed

        }

        /// <summary>
        /// Відправлення інформації про ставку з стандартного адресу
        /// </summary>
        [HttpPost("place/bet")]
        public async Task SendBetMessage()
        {
            await _emailSenderService.SendBetPlace(User.Claims.First(c => c.Type == "userEmail").Value);
        }

        /// <summary>
        /// Відправлення "підтвердження реєстрації" з стандартного адресу
        /// </summary>

        [HttpPost("registration")]
        public async Task SendRegistrationConfirm()
        {
            await _emailSenderService.SendRegistrationConfirm(User.Claims.First(c => c.Type == "userEmail").Value);
        }

        ///// <summary>
        ///// Відправлення інформації про ставку з стандартного адресу
        ///// </summary>
        //[HttpPost("win/bet")]
        //public async Task SendWinBetMessage()
        //{
        //    await _emailSenderService.SendBetWin(User.Claims.First(c => c.Type == "email").Value);
        //}
    }
}
