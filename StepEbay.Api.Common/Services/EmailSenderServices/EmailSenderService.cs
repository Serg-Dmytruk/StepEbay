using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;


namespace StepEbay.Main.Api.Common.Services.EmailSenderServices
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmailSenderService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task SendRegistrationConfirm(string email, string nickname, int id, string emailKey)
        {
            var path = Path.Combine(_webHostEnvironment.WebRootPath, @"templates\_letter-signup.html");
            
            await SendEmail(email, "SmileShop", await GetMailTemplate(path, ("Nickname", nickname), ("ConfirmAddress", _configuration.GetSection("ConfirmAddress").Value),
                ("Id", id.ToString()), ("EmailKey", emailKey)));
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
                MailjetClient client = new MailjetClient(_configuration.GetSection("ApiKey").Value, _configuration.GetSection("SecretKey").Value);

                MailjetRequest request = new MailjetRequest
                {
                    Resource = Send.Resource
                };

                var email = new TransactionalEmailBuilder()
                       .WithFrom(new SendContact("udhdj055@gmail.com"))
                       .WithSubject(title)
                       .WithHtmlPart(description)
                       .WithTo(new SendContact(mail))
                       .Build();

                var response = await client.SendTransactionalEmailAsync(email);              
            });
        }

        private static async Task<string> GetMailTemplate(string templatePath, params (string, string)[] parameters)
        {
            return ((IEnumerable<(string, string)>)parameters).Aggregate<(string, string), string>
                (await File.ReadAllTextAsync(templatePath), (Func<string, (string, string), string>)
                ((current, param) => current.Replace("{" + param.Item1 + "}", param.Item2)));
        }
    }
}
