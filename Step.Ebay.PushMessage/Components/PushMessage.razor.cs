using Microsoft.AspNetCore.Components;
using StepEbay.PushMessage.Configure;

namespace StepEbay.PushMessage.Components
{
    public partial class PushMessage
    {
        [CascadingParameter] private PushMessageContainer Container { get; set; }
        [Parameter] public Guid MessageId { get; set; }
        [Parameter] public MessageSettings Settings { get; set; }

        private void Close()
        {
            Container.RemoveMessage(MessageId);
        }
    }
}