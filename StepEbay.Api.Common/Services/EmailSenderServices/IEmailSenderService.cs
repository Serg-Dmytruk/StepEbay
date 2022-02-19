using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Main.Api.Common.Services.EmailSenderServices
{
    public interface IEmailSenderService
    {
        public Task SendEmail(string mail, string title, string description);
    }
}
