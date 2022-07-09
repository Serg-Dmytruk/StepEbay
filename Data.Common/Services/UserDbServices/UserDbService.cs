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

        public async Task<bool> AnyByEmail(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        public async Task<User> GetUserByNickName(string nickName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.NickName == nickName);
        }

        public async Task<bool> ConfirmEmail(int id, string key)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == id && x.EmailKey == key);

            if (user != null && !user.IsEmailConfirmed)
            {
                user.IsEmailConfirmed = true;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

           return user != null;
        }
    }
}
