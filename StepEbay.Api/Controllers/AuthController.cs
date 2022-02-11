using Microsoft.AspNetCore.Mvc;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Common.Models.Auth;
using Swashbuckle.AspNetCore.Annotations;

namespace StepEbay.Main.Api.Controllers
{
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        public AuthController()
        {

        }

        /// <summary>
        /// Реєстрація користувача
        /// </summary>
        [HttpPost("signup")]
        [SwaggerResponse(200, Type = typeof(SignInResponseDto))]
        public async Task<ResponseData<SignInResponseDto>> SignUp(SignUpRequestDto request)
        {
            //TODO add auth service
            // return await _authService.SignUp(request);
            return null;
        }


        /// <summary>
        /// Авторизація користувача
        /// </summary>
        [HttpPost("signin")]
        [SwaggerResponse(200, Type = typeof(SignInResponseDto))]
        public async Task<ResponseData<SignInResponseDto>> SignIn(SignInRequestDto request)
        {
            //eeturn await _authService.SignIn(request);
            return null;
        }
    }
}
