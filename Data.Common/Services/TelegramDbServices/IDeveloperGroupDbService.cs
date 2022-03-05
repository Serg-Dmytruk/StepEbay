using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Telegram;

namespace StepEbay.Data.Common.Services.TelegramDbServices
{
    public interface IDeveloperGroupDbService : IDefaultDbService<int, DeveloperGroup>
    {
        Task<List<DeveloperGroup>> GetAll();
    }
}
