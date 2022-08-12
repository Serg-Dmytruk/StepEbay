namespace StepEbay.Main.Common.Models.Person
{
    public class PersonUpdateRequestDto
    {
        public string OldPasswordForConfirm { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
        public string FullName { get; set; }
        public string Adress { get; set; }
    }
}
