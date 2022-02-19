using Microsoft.AspNetCore.Components;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Client.Base.Layout;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Auth;
using System.Net;

namespace StepEbay.Main.Client.Base.Pages
{
    [Route("/signup")]
    [Layout(typeof(EmptyLayout))]
    public partial class SignUp
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] IApiService _apiService { get; set; }

        private SignUpRequestDto _signUpRequestDto { get; set; } = new();
        private bool _showPreloader { get; set; } = true;

        private Dictionary<string, List<string>> _errors = new();
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _showPreloader = false;
                StateHasChanged();
            }
        }

        private async Task SignUpRequest()
        {
            //Вова зробити тут валідацію моделькі також (перед відправкою на апі)

            _showPreloader = true;
            _errors = null;

            ResponseData<SignInResponseDto> response =  await _apiService.ExecuteRequest(() => _apiService.ApiMethods.SignUp(_signUpRequestDto));

            if (response.StatusCode == HttpStatusCode.OK)
                NavigationManager.NavigateTo("/main");

            _errors = response.Errors;

            _showPreloader = false;
            StateHasChanged();
        }
    }
}
