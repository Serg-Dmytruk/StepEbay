using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Main.Client.Base.Components
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
