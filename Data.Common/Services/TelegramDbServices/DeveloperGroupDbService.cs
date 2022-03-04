using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Common.Services.UserDbServices;
using StepEbay.Data.Models.Telegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StepEbay.Data.Common.Services.TelegramDbServices
{
    public class DeveloperGroupDbService : DefaultDbService<int, DeveloperGroup>, IDeveloperGroupDbService
    {

        private readonly ApplicationDbContext _context;
        public DeveloperGroupDbService(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }
        

        async Task<List<DeveloperGroup>> IDeveloperGroupDbService.GetAll()
        {
            return await _context.DeveloperGroups.ToListAsync();
        }
    }
}
