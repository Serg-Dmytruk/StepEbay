using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Auth;

namespace StepEbay.Data.Common.Services.AuthDbServices
{
    public class RefreshTokenDbService : DefaultDbService<int, RefreshToken>, IRefreshTokenDbService
    {
        private readonly ApplicationDbContext _context;
        public RefreshTokenDbService(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public async Task<RefreshToken> GetByUserId(int userId, string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId && x.Token == token);
        }

        public async Task<bool> Any(int userId, string token)
        {
            return await _context.RefreshTokens.AnyAsync(x => x.UserId == userId && x.Token == token);
        }

        public async Task RemoveRefreshToken(int userId, string token)
        {
            RefreshToken toRemove = await _context.RefreshTokens.SingleAsync(x => x.UserId == userId && x.Token == token);
            _context.RefreshTokens.Remove(toRemove);
            await _context.SaveChangesAsync();
        }
    }
}
