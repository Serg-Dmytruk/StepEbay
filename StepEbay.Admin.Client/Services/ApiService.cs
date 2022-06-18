using Microsoft.Extensions.Configuration;
using Refit;
using StepEbay.Admin.Client.Common.Providers;
using StepEbay.Admin.Client.Common.RestServices;
using StepEbay.Admin.Common.Models.Auth;
using StepEbay.Common.ApiServices;
using StepEbay.Common.Lockers;
using StepEbay.Common.Models.Auth;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace StepEbay.Admin.Client.Services
{
    public class ApiService : DefaultApiService<IApiMethods>, IApiService
    {
        public ApiService(IConfiguration configuration, ITokenProvider authProvider, SemaphoreManager manager,
             HttpClient httpClient)
            : base(authProvider, manager.Get("API"), httpClient, configuration.GetConnectionString("Api"))
        {

        }

        protected override async Task<(RefreshTokenData, HttpStatusCode)> Refresh(RefreshTokenData data)
        {
            ApiResponse<RefreshTokenResponseDto> response =
                await ApiMethods.RefreshToken(new RefreshTokenRequestDto
                {
                    AccessToken = data.AccessToken,
                    RefreshToken = data.RefreshToken
                });

            return new(new RefreshTokenData
            {
                AccessToken = response.Content?.AccessToken,
                RefreshToken = response.Content?.RefreshToken
            }, response.StatusCode);
        }
    }
}
