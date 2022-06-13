using Microsoft.AspNetCore.Components;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Client.Base.Layout;
using StepEbay.Main.Client.Common.DataValidationServices;
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
        [Inject] IApiService ApiService { get; set; }

        private SignUpRequestDto SignUpRequestDto { get; set; } = new();
        private bool ShowPreloader { get; set; } = true;

        private Dictionary<string, List<string>> _errors = new();

        private bool ShowModal { get; set; } = false;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                ShowPreloader = false;
                StateHasChanged();
            }
        }

        private async Task SignUpRequest()
        {
            ShowPreloader = true;
            //Вова зробити тут валідацію моделькі також (перед відправкою на апі)
            var validator = new AuthValidator();
            var result = await validator.ValidateAsync(SignUpRequestDto);

            if (!result.IsValid)
            {
                var list = new List<string>();
                result.Errors.ForEach(error => list.Add(error.ToString()));
                _errors.Add("Registration", list);
            }
            else
            {
                ResponseData<SignInResponseDto> response = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.SignUp(SignUpRequestDto));

                if (response.StatusCode == HttpStatusCode.OK)
                    NavigationManager.NavigateTo("/main");

                _errors = response.Errors;
            }

            if (_errors.Count > 0)
                ShowModal = true;

            ShowPreloader = false;
            StateHasChanged();
        }

        private void CloseModal(bool show)
        {
            ShowModal = show;
            StateHasChanged();
        }
    }
}
