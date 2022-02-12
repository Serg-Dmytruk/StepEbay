namespace StepEbay.Main.Common.Models.Auth
{
    public class SignUpRequestDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
