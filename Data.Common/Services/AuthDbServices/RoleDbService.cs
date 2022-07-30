using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Auth;

namespace StepEbay.Data.Common.Services.AuthDbServices
{
    public class RoleDbService : DefaultDbService<int, Role>, IRoleDbService
    {
        private readonly ApplicationDbContext _context;
        public RoleDbService(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<bool> AnyByName(string name)
        {
            return await _context.Roles.AnyAsync(role => role.Name == name);
        }

        public async Task<Role> GetByName(string name)
        {
            return await _context.Roles.SingleOrDefaultAsync(role => role.Name == name);
        }

        public async Task<List<string>> GetUserRoleNames(int userId)
        {
            return await _context.UserRoles.Include(x => x.Role).Where(x => x.UserId == userId).Select(r => r.Role.Name).ToListAsync();
        }
    }
}
