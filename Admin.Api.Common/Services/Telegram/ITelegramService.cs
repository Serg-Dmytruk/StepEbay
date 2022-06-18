using StepEbay.Admin.Api.Common.Models;
using StepEbay.Data.Models.Telegram;

namespace StepEbay.Admin.Api.Services.Telegram
{
    public interface ITelegramService
    {
        public Task SendErrorMessage(List<Event> exceptionInfo, string projectName);
        public Task SaveGroup(string group);
        public Task RemoveGroup(int id);
        public Task RemoveGroup(string token);
        public Task UpdateGroup(int id, string newToken);
        public Task UpdateGroup(string oldToken, string newToken);
        public Task<List<DeveloperGroup>> GetAllGroups();
    }
}
