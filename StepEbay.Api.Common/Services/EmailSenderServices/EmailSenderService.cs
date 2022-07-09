using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Mail;

namespace StepEbay.Main.Api.Common.Services.EmailSenderServices
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        public EmailSenderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //TODO IMPLEMENTATION
        public async Task SendRegistrationConfirm(string email, Guid guid)
        {
            await SendEmail(email, "Confirm your account", "Complete registration <a href=\"~/\">here</a>"+guid.ToString());
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
            await Task.Run(async () =>
            { 
                MailjetClient client = new MailjetClient(_configuration.GetSection("ApiKey").Value, _configuration.GetSection("SecretKey").Value) {
                    //Version = ApiVersion.V3_1, 
                };
                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource
                };

                // construct your email with builder
                var email = new TransactionalEmailBuilder()
                       .WithFrom(new SendContact("udhdj055@gmail.com"))
                       .WithSubject(title)
                       .WithHtmlPart(description)
                       .WithTo(new SendContact(mail))
                       .Build();

                // invoke API to send email
                var response = await client.SendTransactionalEmailAsync(email);
                
            });
        }
    }
}
