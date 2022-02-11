namespace StepEbay.Main.Common.Models.Auth
{
    public class SignInResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expires { get; set; }
    }
}
