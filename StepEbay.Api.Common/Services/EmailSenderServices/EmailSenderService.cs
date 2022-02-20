using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace StepEbay.Main.Api.Common.Services.EmailSenderServices
{
    public class EmailSenderService : IEmailSenderService
    {
        IConfiguration _configuration;
        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //TODO IMPLEMENTATION
        public async Task SendRegistrationConfirm(string email)
        {
            //await SendEmail();
        }

        public async Task SendBetPlace(string email)
        {
            //await SendEmail();
        }

        public async Task SendNews(string title, string description)
        {
            //await SendEmail();
        }

        //public async Task SendBetWin(string email)
        //{
        //    //await SendEmail();
        //}

        private async Task SendEmail(string mail, string title, string description)
        {
            await Task.Run(() =>
            {
                string to = mail;
                string from = _configuration.GetSection("Email").Value;
                string fromPassword = _configuration.GetSection("EmailPassword").Value;
                MailMessage message = new MailMessage(from, to);
                message.Subject = title;
                message.Body = description;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
                {
                    DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    Credentials = new NetworkCredential(from, fromPassword),
                };
                client.Send(message);
            });
        }
    }
}
