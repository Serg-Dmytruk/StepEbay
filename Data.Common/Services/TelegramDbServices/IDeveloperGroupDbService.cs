using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Telegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Data.Common.Services.TelegramDbServices
{
    public interface IDeveloperGroupDbService : IDefaultDbService<int, DeveloperGroup>
    {
        Task<List<DeveloperGroup>> GetAll();
    }
}
