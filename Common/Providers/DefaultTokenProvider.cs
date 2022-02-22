using Microsoft.AspNetCore.Components.Authorization;
using StepEbay.Common.Helpers;
using StepEbay.Common.Storages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace StepEbay.Common.Providers
{
    public class DefaultTokenProvider : AuthenticationStateProvider
    {
        private readonly CookieStorage _cookieStorage;

        private readonly string _accessTokenName;
        private readonly string _refreshTokenName;
        private readonly string _expiresName;
        private readonly string _cookieDomain;

        public DefaultTokenProvider(CookieStorage cookieStorage,
            string accessTokenName = "accessToken", string refreshTokenName = "refreshToken",
            string expiresName = "expires", string cookieDomain = "")
        {
            _cookieStorage = cookieStorage;
            _accessTokenName = accessTokenName;
            _refreshTokenName = refreshTokenName;
            _expiresName = expiresName;
            _cookieDomain = cookieDomain;
        }

        public virtual async Task SetToken(string token, string refreshToken, DateTime expires,
            int maxAge = 86400, int refreshMaxAge = 604800, int expiresMaxAge = 86400, int expiresCheck = 60)
        {
            await _cookieStorage.SetCookie(_accessTokenName, token, maxAge, _cookieDomain);
            await _cookieStorage.SetCookie(_refreshTokenName, refreshToken, refreshMaxAge, _cookieDomain);
            await _cookieStorage.SetCookie(_expiresName, expires.AddSeconds(-expiresCheck).ToUnix().ToString(), expiresMaxAge, _cookieDomain);
        }

        public virtual async Task SetSessionToken(string token, string refreshToken, DateTime expires, int expiresCheck = 60)
        {
            await _cookieStorage.SetSessionCookie(_accessTokenName, token, _cookieDomain);
            await _cookieStorage.SetSessionCookie(_refreshTokenName, refreshToken, _cookieDomain);
            await _cookieStorage.SetSessionCookie(_expiresName, expires.AddSeconds(-expiresCheck).ToUnix().ToString(), _cookieDomain);
        }

        public virtual async Task<string> GetToken() => await _cookieStorage.GetCookie(_accessTokenName);
        public virtual async Task<string> GetRefreshToken() => await _cookieStorage.GetCookie(_refreshTokenName);

        public virtual async Task<bool> GetRememberMe()
        {
            if (bool.TryParse(await _cookieStorage.GetCookie("rememberme"), out bool rememberMe))
                return rememberMe;

            return false;
        }

        public virtual async Task<long> GetExpires()
        {
            if (long.TryParse(await _cookieStorage.GetCookie(_expiresName), out long expires))
                return expires;

            return 0;
        }

        public virtual async Task RemoveToken()
        {
            await _cookieStorage.RemoveCookie(_accessTokenName, _cookieDomain);
        }

        public virtual async Task CheckAuthentication(bool auth)
        {
            AuthenticationState state = await GetAuthenticationStateAsync();

            if (auth && state.User.Identity.IsAuthenticated
                || !auth && !state.User.Identity.IsAuthenticated)
            {
                NotifyAuthenticationStateChanged(Task.FromResult(state));
            }
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string token = await GetToken();
            IIdentity identity = string.IsNullOrEmpty(token)
                ? new ClaimsIdentity()
                : GetClaimsPrincipal(token).Identity;
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }

        private static ClaimsPrincipal GetClaimsPrincipal(string token)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(GetClaims(token), "jwt",
                ClaimTypes.Name, ClaimsIdentity.DefaultRoleClaimType));
        }

        private static List<Claim> GetClaims(string token)
        {
            JwtSecurityTokenHandler handler = new();

            if (handler.ReadJwtToken(token) is JwtSecurityToken jwtSecurityToken)
                return jwtSecurityToken.Claims.ToList();

            throw new Exception("Token is invalid!");
        }
    }
}
