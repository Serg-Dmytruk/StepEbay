using Refit;
using StepEbay.Main.Common.Models.Auth;

namespace StepEbay.Admin.Client.Common.RestServices
{
    public  interface IApiMethods
    {
        [Post("/auth/refresh")]
        Task<ApiResponse<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto request);
    }
}
