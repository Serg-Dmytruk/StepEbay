using StepEbay.Data.Common.Services.TelegramDbServices;

namespace StepEbay.Admin.Api.Services.Telegram
{
    public interface ITelegramService
    {
        public Task SendErrorMessage(string message, IDeveloperGroupDbService groups);
        public Task SaveGroup(string group, IDeveloperGroupDbService groups);
    }
}
