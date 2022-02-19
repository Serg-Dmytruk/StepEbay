using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Users;

namespace StepEbay.Data.Common.Services.UserDbServices
{
    public class UserDbService : DefaultDbService<int, User>, IUserDbService
    {
        private readonly ApplicationDbContext _context;
        public UserDbService(ApplicationDbContext context) 
            : base(context)
        {
            _context = context;
        }

        public async Task<bool> AnyByNickName(string nickName)
        {
            return await _context.Users.AnyAsync(x => x.NickName == nickName);
        }
    }
}
