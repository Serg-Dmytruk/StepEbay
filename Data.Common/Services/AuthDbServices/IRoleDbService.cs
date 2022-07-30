using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Auth;

namespace StepEbay.Data.Common.Services.AuthDbServices
{ 
    public interface IRoleDbService : IDefaultDbService<int, Role>
    {
        Task<List<string>> GetUserRoleNames(int userId);
        Task<bool> AnyByName(string name);
        Task<Role> GetByName(string name);
    }
}
