using Microsoft.AspNetCore.Mvc;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Api.Common.Services.EmailSenderServices;

namespace StepEbay.Main.Api.Controllers
{
    [Route("email")]
    public class EmailController : ControllerBase
    {
        private IEmailService _emailSenderService;
        public EmailController(IEmailService emailSenderService)
        {
            _emailSenderService = emailSenderService;
        }

        /// <summary>
        /// Відправлення новин з стандартного адресу
        /// </summary>
        [HttpPost("news")]
        public async Task SendNews(string title, string description)
        {
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

        [HttpGet("confirm/{id}/{key}")]
        public async Task<ResponseData<BoolResult>> ConfirmEmail(string id, string key)
        {
            return await _emailSenderService.ConfirmRegistration(int.Parse(id), key);
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
