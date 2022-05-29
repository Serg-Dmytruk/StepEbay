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
        public TelegramService(IConfiguration configuration)
        {
            _botClient = new TelegramBotClient(configuration.GetSection("TelegramBotToken").Value);
        }
        public async Task SendErrorMessage(string message, IDeveloperGroupDbService groups)
        {
            await SendMessage(message, groups);
        }
        public async Task SaveGroup(string group, IDeveloperGroupDbService groups)
        {
            await groups.Add(new DeveloperGroup() { Group = group });
        }
        public async Task RemoveGroup(string token, IDeveloperGroupDbService groups)
        {
            await groups.Remove(groups.GetByToken(token).Result);
        }
        public async Task RemoveGroup(int id, IDeveloperGroupDbService groups)
        {
            await groups.Remove(groups.Get(id).Result);
        }
        public async Task UpdateGroup(int id, string newToken, IDeveloperGroupDbService groups)
        {
            await groups.Update(new DeveloperGroup() { Id=id,Group=newToken});
        }
        public async Task UpdateGroup(string oldToken, string newToken, IDeveloperGroupDbService groups)
        {
            DeveloperGroup _onEditiDeveloperGroup= groups.GetByToken(oldToken).Result;
            _onEditiDeveloperGroup.Group = newToken;
            await groups.Update(_onEditiDeveloperGroup);
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
