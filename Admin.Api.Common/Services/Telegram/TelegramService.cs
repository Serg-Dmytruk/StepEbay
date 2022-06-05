using Microsoft.Extensions.Configuration;
using StepEbay.Data.Common.Services.TelegramDbServices;
using StepEbay.Data.Models.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StepEbay.Admin.Api.Services.Telegram
{
    public class TelegramService : ITelegramService
    {
        private readonly TelegramBotClient _botClient;
        private readonly IDeveloperGroupDbService _groups;

        public TelegramService(IConfiguration configuration, IDeveloperGroupDbService group)
        {
            _botClient = new TelegramBotClient(configuration.GetSection("TelegramBotToken").Value);
            _groups = group;
        }

        public async Task SendErrorMessage(string message)
        {
            await SendMessage(message, _groups);
        }

        public async Task SaveGroup(string group)
        {
            await _groups.Add(new DeveloperGroup() { Group = group });
        }

        public async Task RemoveGroup(string token)
        {
            await _groups.Remove(_groups.GetByToken(token).Result);
        }

        public async Task RemoveGroup(int id)
        {
            await _groups.Remove(_groups.Get(id).Result);
        }

        public async Task UpdateGroup(int id, string newToken)
        {
            await _groups.Update(new DeveloperGroup() { Id=id,Group=newToken});
        }

        public async Task UpdateGroup(string oldToken, string newToken)
        {
            DeveloperGroup _onEditiDeveloperGroup= _groups.GetByToken(oldToken).Result;
            _onEditiDeveloperGroup.Group = newToken;
            await _groups.Update(_onEditiDeveloperGroup);
        }

        public async Task<List<DeveloperGroup>> GetAllGroups()
        {
            return await _groups.GetAll();
        }

        private async Task SendMessage(string message, IDeveloperGroupDbService groups)
        {
            await Task.Run(() =>
            {
                List<DeveloperGroup> list = groups.List().Result;
                foreach (DeveloperGroup group in list)
                {
                    _botClient.SendTextMessageAsync(new ChatId(group.Group), message);
                }
            });
        }
    }
}
