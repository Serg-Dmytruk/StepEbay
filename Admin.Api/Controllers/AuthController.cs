using Microsoft.AspNetCore.Mvc;
using StepEbay.Admin.Api.Common.Services.AuthServices;
using StepEbay.Admin.Common.Models.Auth;
using StepEbay.Common.Models.RefitModels;
using Swashbuckle.AspNetCore.Annotations;

namespace StepEbay.Admin.Api.Controllers
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
        /// Авторизація користувача
        /// </summary>
        [HttpPost("signin")]
        [SwaggerResponse(200, Type = typeof(SignInResponseDto))]
        public async Task<ResponseData<SignInResponseDto>> SignIn([FromBody] SignInRequestDto request)
        {
            return await _authService.SignIn(request);
        }

        /// <summary>
        /// Оновлення авторизаційних токенів
        /// </summary>
        [HttpPost("refresh")]
        public async Task<ResponseData> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            return await _authService.RefreshToken(request);
        }
    }
}
