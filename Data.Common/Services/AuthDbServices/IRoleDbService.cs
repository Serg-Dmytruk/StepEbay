using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Auth;

namespace StepEbay.Data.Common.Services.AuthDbServices
{
    public interface IRoleDbService : IDefaultDbService<int, Role>
    {
        Task<List<string>> GetUserRoleNames(int userId);
    }
}
