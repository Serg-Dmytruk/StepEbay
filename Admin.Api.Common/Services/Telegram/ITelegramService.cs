using StepEbay.Data.Common.Services.TelegramDbServices;

namespace StepEbay.Admin.Api.Services.Telegram
{
    public interface ITelegramService
    {
        public Task SendErrorMessage(string message, IDeveloperGroupDbService groups);
        public Task SaveGroup(string group, IDeveloperGroupDbService groups);
        public Task RemoveGroup(int id, IDeveloperGroupDbService groups);
        public Task RemoveGroup(string token, IDeveloperGroupDbService groups);
        public Task UpdateGroup(int id, string newToken, IDeveloperGroupDbService groups);
        public Task UpdateGroup(string oldToken, string newToken, IDeveloperGroupDbService groups);
    }
}
