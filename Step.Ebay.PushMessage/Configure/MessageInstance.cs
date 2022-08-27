namespace StepEbay.PushMessage.Configure
{
    public class MessageInstance
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public MessageSettings MessageSettings { get; set; }
    }
}
