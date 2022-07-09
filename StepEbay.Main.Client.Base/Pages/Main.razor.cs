using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Common.Providers;
using StepEbay.Main.Client.Common.RestServices;


namespace StepEbay.Main.Client.Base.Pages
{
    [Route("/")]
    [Route("main")]
    [Route("main/confirm/{id}/{key}")]
    public partial class Main
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public string Key { get; set; }

        [Inject] private ITokenProvider TokenProvider { get; set; }
        [Inject] IApiService ApiService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if(string.IsNullOrEmpty(Id) && string.IsNullOrEmpty(Key))
            {

            }

          
        }

        private async Task PlaceBet()
        {
            await ApiService.ExecuteRequest(() => ApiService.ApiMethods.PlaceBet(1));
        }
    }
}
