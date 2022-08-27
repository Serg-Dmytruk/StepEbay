using StepEbay.PushMessage.Configure;

namespace StepEbay.PushMessage.Services
{
    public class MessageService : IMessageService
    {
        public event Action<MessageLevel, string, string> OnShow;

        public void ShowError(string title, string message)
        {
            ShowMessage(MessageLevel.Error, title, message);
        }

        public void ShowInfo(string title, string message)
        {
            ShowMessage(MessageLevel.Info, title, message);
        }

        public void ShowSuccsess(string title, string message)
        {
            ShowMessage(MessageLevel.Success, title, message);
        }

        public void ShowWarning(string title, string message)
        {
             ShowMessage(MessageLevel.Warning, title, message);
        }

        private void ShowMessage(MessageLevel level, string title, string message)
        {
            OnShow?.Invoke(level, title, message);
        }
    }
}
