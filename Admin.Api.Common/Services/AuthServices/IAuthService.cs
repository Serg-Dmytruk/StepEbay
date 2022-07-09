using StepEbay.Admin.Common.Models.Auth;
using StepEbay.Common.Models.RefitModels;

namespace StepEbay.Admin.Api.Common.Services.AuthServices
{
    public interface IAuthService
    {
        Task<ResponseData<SignInResponseDto>> SignIn(SignInRequestDto model);
        Task<ResponseData<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto model);
    }
}
