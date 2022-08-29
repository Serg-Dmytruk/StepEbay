using Microsoft.AspNetCore.Components;
using StepEbay.PushMessage.Configure;
using StepEbay.PushMessage.Services;
using Timer = System.Timers.Timer;

namespace StepEbay.PushMessage.Components
{
    public partial class PushMessageContainer
    {
        [Inject] IMessageService MessageService { get; set; }
        [Parameter] public MessagePosition Position { get; set; } = MessagePosition.BottomRight;
        [Parameter] public int TimeOut { get; set; } = 5;

        private string PositionClass { get; set; } = string.Empty;

        public List<MessageInstance> Messages = new();

        protected override void OnInitialized()
        {
            MessageService.OnShow += ShowMessage;
            PositionClass = $"position-{Position.ToString().ToLower()}";
        }

        public void RemoveMessage(Guid messageId)
        {
            InvokeAsync(() =>
            {
                var messageInstance = Messages.SingleOrDefault(x => x.Id == messageId);

                if (messageInstance != null)
                    Messages.Remove(messageInstance);

                StateHasChanged();
            });
        }

        private MessageSettings SetSettings(MessageLevel level, string title, string message) => level switch
        {
            MessageLevel.Info =>
                new MessageSettings(string.IsNullOrWhiteSpace(title) ? "Info" : title, message, "blazored-toast-info"),
            MessageLevel.Success =>
               new MessageSettings(string.IsNullOrWhiteSpace(title) ? "Success" : title, message, "blazored-toast-success"),
            MessageLevel.Warning =>
               new MessageSettings(string.IsNullOrWhiteSpace(title) ? "Warning" : title, message, "blazored-toast-warning"),
            MessageLevel.Error =>
               new MessageSettings(string.IsNullOrWhiteSpace(title) ? "Error" : title, message, "blazored-toast-error"),
            _ => throw new InvalidOperationException("PushMessage.SetSettings: level exception")
        };

        private void ShowMessage(MessageLevel level, string title, string message)
        {
            InvokeAsync(() =>
            {
                var messageInstance = new MessageInstance
                {
                    Id = Guid.NewGuid(),
                    TimeStamp = DateTime.UtcNow,
                    MessageSettings = SetSettings(level, title, message)
                };

                Messages.Add(messageInstance);

                var timeout = TimeOut * 1000;
                var messageTimer = new Timer(timeout);

                messageTimer.Elapsed += (sender, args) => { RemoveMessage(messageInstance.Id); };
                messageTimer.AutoReset = false;
                messageTimer.Start();

                StateHasChanged();
            });
        }
    }
}