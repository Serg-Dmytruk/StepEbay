using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using StepEbay.Admin.Api.Common.Models;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.TelegramDbServices;
using StepEbay.Data.Models.Telegram;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace StepEbay.Admin.Api.Services.Telegram
{
    public class TelegramService : ITelegramService
    {
        private readonly TelegramBotClient _botClient;
        private readonly IDeveloperGroupDbService _groups;
        private readonly ILogger<TelegramService> _logger;

        public TelegramService(IConfiguration configuration, IDeveloperGroupDbService group, ILogger<TelegramService> logger)
        {
            _botClient = new TelegramBotClient(configuration.GetSection("TelegramBotToken").Value);
            _groups = group;
            _logger = logger;
        }

        public async Task<BoolResult> SendErrorMessage(List<Event> exceptionInfo, string projectName)
        {
            await SendMessage(exceptionInfo, projectName, _groups);
            return new BoolResult(true);
        }

        public async Task<BoolResult> AddGroup(string group)
        {
            await _groups.Add(new DeveloperGroup() { Group = group });
            return new BoolResult(true);
        }

        public async Task<BoolResult> RemoveGroup(string token)
        {
            await _groups.Remove(_groups.GetByToken(token).Result);
            return new BoolResult(true);
        }

        public async Task<BoolResult> RemoveGroup(int id)
        {
            await _groups.Remove(_groups.Get(id).Result);
            return new BoolResult(true);
        }

        public async Task<BoolResult> UpdateGroup(int id, string newToken)
        {
            await _groups.Update(new DeveloperGroup() { Id = id, Group = newToken });
            return new BoolResult(true);
        }

        public async Task<BoolResult> UpdateGroup(string oldToken, string newToken)
        {
            DeveloperGroup _onEditiDeveloperGroup = _groups.GetByToken(oldToken).Result;
            _onEditiDeveloperGroup.Group = newToken;
            await _groups.Update(_onEditiDeveloperGroup);
            return new BoolResult(true);
        }

        public async Task<List<DeveloperGroup>> GetAllGroups()
        {
            return await _groups.GetAll();
        }

        private async Task SendMessage(List<Event> exceptionInfo, string projectName, IDeveloperGroupDbService groups)
        {
            try
            {
                List<DeveloperGroup> list = groups.List().Result;
                foreach (DeveloperGroup group in list)
                {
                    await _botClient.SendTextMessageAsync(new ChatId(group.Group), CreateErrorMessage(exceptionInfo, projectName));
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
            }
        }

        private string CreateErrorMessage(List<Event> exceptionInfo, string projectName)
        {
            StringBuilder mess = new();
            foreach (var exception in exceptionInfo)
            {
                mess.Clear();
                mess.AppendLine($"----------------------------- {projectName} -----------------------------");
                mess.AppendLine($"Time: {exception.Timestamp.UtcDateTime.ToString("dd.MM.yyyy HH:mm:ss")}");

                if (string.IsNullOrEmpty(exception.Exception))
                    mess.AppendLine($"Exception: {exception.RenderedMessage}");
                else
                    mess.AppendLine($"Exception: {exception.Exception}");
            }

            return mess.ToString().Length > 2000 ? mess.ToString()[..1999] : mess.ToString();
        }
    }
}
