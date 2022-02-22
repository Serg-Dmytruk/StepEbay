using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Common.Providers
{
    public interface IDefaultTokenProvider
    {
        Task SetToken(string token, string refreshToken, DateTime expires, int maxAge = 86400, int refreshMaxAge = 604800, int expiresMaxAge = 86400, int expiresCheck = 60);
        Task SetSessionToken(string token, string refreshToken, DateTime expires, int expiresCheck = 60);
        Task<string> GetToken();
        Task<string> GetRefreshToken();
        Task<bool> GetRememberMe();
        Task<long> GetExpires();
        Task RemoveToken();
        Task CheckAuthentication(bool auth);
        Task<AuthenticationState> GetAuthenticationStateAsync();
    }
}
