using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Common.Models.Auth;

namespace StepEbay.Main.Api.Common.Services.AuthServices
{
    public interface IAuthService
    {
        Task<ResponseData<SignInResponseDto>> SignIn(SignInRequestDto model);
        Task<ResponseData<SignInResponseDto>> SignUp(SignUpRequestDto model);
        Task<ResponseData<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto model);
    }
}
