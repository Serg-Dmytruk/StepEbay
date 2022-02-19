using Microsoft.AspNetCore.Mvc;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Api.Common.Services.AuthServices;
using StepEbay.Main.Common.Models.Auth;
using Swashbuckle.AspNetCore.Annotations;

namespace StepEbay.Main.Api.Controllers
{
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Реєстрація користувача
        /// </summary>
        [HttpPost("signup")]
        [SwaggerResponse(200, Type = typeof(SignInResponseDto))]
        public async Task<ResponseData<SignInResponseDto>> SignUp(SignUpRequestDto request)
        {
            return await _authService.SignUp(request);
        }

        /// <summary>
        /// Авторизація користувача
        /// </summary>
        [HttpPost("signin")]
        [SwaggerResponse(200, Type = typeof(SignInResponseDto))]
        public async Task<ResponseData<SignInResponseDto>> SignIn(SignInRequestDto request)
        {
            return await _authService.SignIn(request);
        }

        /// <summary>
        /// Оновлення авторизаційних токенів
        /// </summary>
        [HttpPost("refresh")]
        public async Task<ResponseData> RefreshToken(RefreshTokenRequestDto request)
        {
            return await _authService.RefreshToken(request);
        }
    }
}
