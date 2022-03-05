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
        public async Task SendErrorMessage(string _message, IDeveloperGroupDbService _groups)
        {
            await SendMessage(_message, _groups);
        }
        public async Task SaveGroup(string group, IDeveloperGroupDbService _groups)
        {
            await _groups.Add(new DeveloperGroup() { Group = group });
        }

        private async Task SendMessage(string message, IDeveloperGroupDbService _groups)
        {
            await Task.Run(() =>
            {
                List<DeveloperGroup> list = _groups.List().Result;
                foreach (DeveloperGroup group in list)
                {
                    _botClient.SendTextMessageAsync(new ChatId(group.Group), message);
                }
            });
        }
    }
}
