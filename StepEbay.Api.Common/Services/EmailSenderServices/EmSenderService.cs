using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Main.Api.Common.Services.EmailSenderServices
{
    internal class EmSenderService:IEmSenderService
    {
        public async Task SendEmail(string mail, string title, string description)
        {
            await Task.Run(() => {
                string to = mail;
                string from = "lazokivan0@gmail.com";
                string fromPassword = "254478254478";
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
