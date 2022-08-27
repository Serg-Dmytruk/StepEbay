using StepEbay.PushMessage.Configure;

namespace StepEbay.PushMessage.Services
{
    public interface IMessageService
    {
        event Action<MessageLevel, string, string> OnShow;
        void ShowInfo(string title, string message);
        void ShowSuccsess(string title, string message);
        void ShowWarning(string title, string message);
        void ShowError(string title, string message);
    }
}
