using Refit;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Common.Models.Auth;
using StepEbay.Main.Common.Models.Person;

namespace StepEbay.Main.Client.Common.RestServices
{
    public interface IApiMethods
    {
        [Post("/auth/signin")]
        Task<ApiResponse<SignInResponseDto>> SignIn(SignInRequestDto request);

        [Post("/auth/signup")]
        Task<ApiResponse<SignInResponseDto>> SignUp(SignUpRequestDto request);

        [Post("/auth/refresh")]
        Task<ApiResponse<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto request);

        [Post("/bet/place/{lotId}")]
        Task<ApiResponse<BoolResult>> PlaceBet(int lotId);

        [Get("/email/confirm/{id}/{key}")]
        Task<ApiResponse<BoolResult>> ConfirmRegistration(string id, string key);

        [Post("/person/update/{passwordconfirm}/{nick}/{email}/{password}/{repassword}/{name}/{adress}")]
        Task<ApiResponse<BoolResult>> TryUpdatePerson(string passwordconfirm, string nick, string email, string password, string repassword, string name, string adress);

        [Get("/person/get")]
        Task<ResponseData<PersonResponseDto>> GetPersonToUpdateInCabinet();
    }
}

