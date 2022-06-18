using Refit;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Common.Models.Auth;

namespace StepEbay.Admin.Client.Common.RestServices
{
    public interface IApiMethods
    {
        [Put("/error/group/add/{groupTg}")]
        Task<ApiResponse<BoolResult>> AddGroup(string groupTg);

        [Post("/auth/refresh")]
        Task<ApiResponse<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto request);
    }
}
