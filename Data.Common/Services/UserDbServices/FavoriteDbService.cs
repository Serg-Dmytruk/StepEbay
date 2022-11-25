using Microsoft.EntityFrameworkCore;
using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Users;

namespace StepEbay.Data.Common.Services.UserDbServices
{
    public class FavoriteDbService : DefaultDbService<int, Favorite>, IFavoriteDbService
    {
        private readonly ApplicationDbContext _context;
        public FavoriteDbService(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }
        public async Task<bool> ToggleFavorite(int productId, int userId)
        {
            if(await _context.Favorites.AnyAsync(n => n.ProductId == productId && n.UserId == userId))
            {
                _context.Favorites.Remove(await _context.Favorites.FirstOrDefaultAsync(n => n.UserId == userId && n.ProductId == productId));
                await _context.SaveChangesAsync();
                return false;
            }
            _context.Add(new Favorite() { ProductId=productId, UserId=userId});
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> IsFavorite(int productId, int userId)
        {
            return await _context.Favorites.AnyAsync(n => n.ProductId == productId && n.UserId == userId);
        }
    }
}
