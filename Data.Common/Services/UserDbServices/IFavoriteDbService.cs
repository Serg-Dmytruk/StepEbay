using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Users;

namespace StepEbay.Data.Common.Services.UserDbServices
{
    public interface IFavoriteDbService : IDefaultDbService<int, Favorite>
    {
        public Task<bool> ToggleFavorite(int productId, int userId);
        public Task<bool> IsFavorite(int productId, int userId);
        public Task<List<Favorite>> GetAllFavorite(int userId);
    }
}
