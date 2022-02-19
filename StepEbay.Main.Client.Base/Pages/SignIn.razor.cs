using Microsoft.AspNetCore.Components;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Client.Base.Layout;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Auth;
using System.Net;

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
        private SignInResponseDto _signInResponseDto { get; set; } = new();
        private bool _showPreloader { get; set; } = true;      
        private bool _rememberMe { get; set; }

        private Dictionary<string, List<string>> _errors = new();

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _showPreloader = false;
                StateHasChanged();
            }           
        }

        private async Task SignInRequest()
        {
            _showPreloader = true;

            _errors = new();
            ResponseData<SignInResponseDto> response = await _apiService.ExecuteRequest(()=> _apiService.ApiMethods.SignIn(_signInRequestDto));

            if (response.StatusCode == HttpStatusCode.OK)
                _navigationManager.NavigateTo("/main");

            _errors = response.Errors;

            _showPreloader = false;
            StateHasChanged();
        }

    }
}
