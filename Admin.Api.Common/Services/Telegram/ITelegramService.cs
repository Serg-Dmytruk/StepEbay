using StepEbay.Admin.Api.Common.Models;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Models.Telegram;

namespace StepEbay.Admin.Api.Services.Telegram
{
    public interface ITelegramService
    {
        public Task<BoolResult> SendErrorMessage(List<Event> exceptionInfo, string projectName);
        public Task<BoolResult> AddGroup(string group);
        public Task<BoolResult> RemoveGroup(int id);
        public Task<BoolResult> RemoveGroup(string token);
        public Task<BoolResult> UpdateGroup(int id, string newToken);
        public Task<BoolResult> UpdateGroup(string oldToken, string newToken);
        public Task<List<DeveloperGroup>> GetAllGroups();
    }
}
