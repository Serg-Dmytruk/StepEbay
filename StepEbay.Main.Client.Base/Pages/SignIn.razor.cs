using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Base.Layout;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Auth;

namespace StepEbay.Main.Client.Base.Pages
{
    [Route("/")]
    [Route("/signin")]
    [Layout(typeof(EmptyLayout))]
    public partial class SignIn
    {
        [Inject] NavigationManager _navigationManager { get; set; }
        [Inject] IApiService _apiService { get; set; }
        private SignInRequestDto _signInRequestDto { get; set; } = new();
        private bool _showPreloader { get; set; } = true;      
        private bool _rememberMe { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _showPreloader = false;
                StateHasChanged();
            }
            
        }
        private async Task SigninRequest()
        {
            _showPreloader = true;
            await _apiService.ExecuteRequest(()=> _apiService.ApiMethods.SignIn(_signInRequestDto));
            _showPreloader = false;
            StateHasChanged();
        }

    }
}
