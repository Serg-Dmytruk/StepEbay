using StepEbay.Data.Common.Services.TelegramDbServices;
using StepEbay.Data.Models.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StepEbay.Admin.Api.Services.Telegram
{
    public class TelegramService : ITelegramService
    {
        private readonly TelegramBotClient botClient;
        public TelegramService()
        {
            botClient = new TelegramBotClient("5298717946:AAFFyaDpBPFj3jcPmwrQIhXy-LQs1DletX8");
            
        }
        public async Task SendErrorMessage(string _message, IDeveloperGroupDbService _groups)
        {
            await SendMessage(_message, _groups);
        }
        public async Task SaveGroup(string group, IDeveloperGroupDbService _groups)
        {
            await _groups.Add(new DeveloperGroup(){Group = group });
        }

        private async Task SendMessage(string message, IDeveloperGroupDbService _groups)
        {
            await Task.Run(() =>
            {
                List<DeveloperGroup> list=_groups.List().Result;
                foreach (DeveloperGroup group in list)
                {
                    botClient.SendTextMessageAsync(new ChatId(group.Group), message);
                    Console.WriteLine("Group: " + group.Group + ": \"" + message + "\"");
                }
            });
        }
    }
}
