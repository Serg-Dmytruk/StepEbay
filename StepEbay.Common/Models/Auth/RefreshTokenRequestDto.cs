﻿namespace StepEbay.Main.Common.Models.Auth
{
    public class RefreshTokenRequestDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
