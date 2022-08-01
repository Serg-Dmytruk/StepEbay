namespace StepEbay.Main.Common.Models.Person
{
    public class PersonUpdateResponseDto
    {
        public bool IsSuccess { get; set; }
        public List<string> FailuresList { get; set; }
    }
}
