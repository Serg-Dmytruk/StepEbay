using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Auth;

namespace StepEbay.Data.Common.Services.AuthDbServices
{
    public class UserRoleDbService : DefaultDbService<int, UserRole>, IUserRoleDbService
    {
        private readonly ApplicationDbContext _context;
        public UserRoleDbService(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
