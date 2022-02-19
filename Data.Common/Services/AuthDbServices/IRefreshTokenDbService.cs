using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Auth;

namespace StepEbay.Data.Common.Services.AuthDbServices
{
    public interface IRefreshTokenDbService : IDefaultDbService<int, RefreshToken>
    {
        Task<RefreshToken> GetByUserId(int userId, string token);
        Task<bool> Any(int userId, string token);
        Task RemoveRefreshToken(int userId, string token);
    }
}
