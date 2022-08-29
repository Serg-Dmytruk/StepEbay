namespace StepEbay.PushMessage.Configure
{
    public class MessageSettings
    {
        public readonly string Title;
        public readonly string Message;
        public readonly string BaseClass;

        public MessageSettings(string title, string message, string baseClass)
        {
            Title = title;
            Message = message;
            BaseClass = baseClass;
        }
    }
}
