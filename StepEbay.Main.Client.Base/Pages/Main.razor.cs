using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Common.Providers;
using StepEbay.Main.Client.Common.RestServices;


namespace StepEbay.Main.Client.Base.Pages
{
    [Route("/main")]
    [Authorize]
    public partial class Main
    {
        [Inject] private ITokenProvider TokenProvider { get; set; }
        [Inject] IApiService ApiService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            
        }

        private async Task PlaceBet()
        {
            await ApiService.ExecuteRequest(() => ApiService.ApiMethods.PlaceBet(1));
        }
    }
}
