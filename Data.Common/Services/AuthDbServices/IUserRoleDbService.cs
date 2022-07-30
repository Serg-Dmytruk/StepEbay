using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Auth;

namespace StepEbay.Data.Common.Services.AuthDbServices
{
    public interface IUserRoleDbService : IDefaultDbService<int, UserRole>
    {
    }
}
