using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace StepEbay.Common.Components
{
    public class Redirect : ComponentBase
    {
        [Parameter]
        public string Url { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        protected override void OnAfterRender(bool firstRender) => this.NavigationManager.NavigateTo(this.Url, true);

        protected override void BuildRenderTree(RenderTreeBuilder __builder)
        {
        }
    }
}
