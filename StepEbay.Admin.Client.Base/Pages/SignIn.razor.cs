using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using StepEbay.Admin.Client.Base.Layout;
using StepEbay.Admin.Client.Common.Options;
using StepEbay.Admin.Client.Common.Providers;
using StepEbay.Admin.Client.Common.RestServices;
using StepEbay.Admin.Common.Models.Auth;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Common.Storages;
using System.Net;

namespace StepEbay.Admin.Client.Base.Pages
{
    [Route("/")]
    [Route("/signin")]
    [Layout(typeof(EmptyLayout))]
    public partial class SignIn
    {
        [Inject] private ITokenProvider TokenProvider { get; set; }
        [Inject] IApiService ApiService { get; set; }
        [Inject] private IOptions<DomainOptions> DomainOptions { get; set; }
        [Inject] private LocalStorage LocalStorage { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        private SignInRequestDto SignInRequestDto { get; set; } = new();
        private bool ShowPreloader { get; set; } = true;
        private bool RememberMe { get; set; }
        public bool ShowModal { get; set; } = false;

        private Dictionary<string, List<string>> _errors = new();


        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                ShowPreloader = false;
                StateHasChanged();
            }
        }

        private async Task SignInRequest()
        {
            ShowPreloader = true;
            _errors = new();
            ResponseData<SignInResponseDto> response = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.SignIn(SignInRequestDto));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (RememberMe)
                    await TokenProvider.SetToken(response.Data.AccessToken, response.Data.RefreshToken, response.Data.Expires);
                else
                    await TokenProvider.SetSessionToken(response.Data.AccessToken, response.Data.RefreshToken, response.Data.Expires);


                var authState = await TokenProvider.GetAuthenticationStateAsync();
                if(!authState.User.IsInRole("admin") && !authState.User.IsInRole("manager"))
                {
                    var nonInRoles = new List<string>();

                    if (!authState.User.IsInRole("admin"))
                        nonInRoles.Add("admin");

                    if (!authState.User.IsInRole("manager"))
                        nonInRoles.Add("manager");

                    _errors.Add("Відсутні парва", nonInRoles);

                    ShowModal = true;
                    await TokenProvider.RemoveToken();
                    await TokenProvider.CheckAuthentication(false);
                    ShowPreloader = false;
                    StateHasChanged();
                    return;
                }

                await LocalStorage.SetLocal("username", SignInRequestDto.NickName);

                await TokenProvider.CheckAuthentication(true);

                NavigationManager.NavigateTo("/main");

                return;
            }

            _errors = response.Errors;

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
