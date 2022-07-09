using Microsoft.AspNetCore.Components;

namespace StepEbay.Main.Client.Base.Components.Modals
{
    public partial class ModalDialog
    {
        [Parameter]
        public string Title { get; set; }

        [Parameter]
        public List<string> Text { get; set; }

        [Parameter]
        public EventCallback<bool> OnClose { get; set; }

        private Task ModalOk()
        {
            return OnClose.InvokeAsync(false);
        }
    }
}
