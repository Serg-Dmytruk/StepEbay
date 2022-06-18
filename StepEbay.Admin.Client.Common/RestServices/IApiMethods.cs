using Refit;
using StepEbay.Admin.Common.Models.Auth;
using StepEbay.Common.Models.RefitModels;

namespace StepEbay.Admin.Client.Common.RestServices
{
    public interface IApiMethods
    {
        #region telegram
        [Put("/telegram/group/add/{groupTg}")]
        Task<ApiResponse<BoolResult>> AddGroup(string groupTg);

        #endregion

        [Post("/auth/signin")]
        Task<ApiResponse<SignInResponseDto>> SignIn(SignInRequestDto request);

        [Post("/auth/refresh")]
        Task<ApiResponse<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto request);
    }
}
