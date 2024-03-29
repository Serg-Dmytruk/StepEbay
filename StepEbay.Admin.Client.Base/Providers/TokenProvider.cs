﻿using StepEbay.Admin.Client.Common.Providers;
using StepEbay.Common.Providers;
using StepEbay.Common.Storages;

namespace StepEbay.Admin.Client.Base.Providers
{
    public class TokenProvider : DefaultTokenProvider, ITokenProvider
    {
        public TokenProvider(CookieStorage cookieStorage,
           string accessTokenName = "accessToken", string refreshTokenName = "refreshToken",
           string expiresName = "expires", string cookieDomain = "")

           : base(cookieStorage, accessTokenName, refreshTokenName, expiresName, cookieDomain)
        {

        }
    }
}
