﻿namespace StepEbay.Common.Models.Auth
{
    public class RefreshTokenData
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
    }
}
