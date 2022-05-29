﻿using Microsoft.AspNetCore.Components;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Client.Base.Layout;
using StepEbay.Main.Client.Common.Providers;
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
        [Inject] private ITokenProvider TokenProvider { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        [Inject] IApiService ApiService { get; set; }
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
            ResponseData<SignInResponseDto> response = await ApiService.ExecuteRequest(()=> ApiService.ApiMethods.SignIn(SignInRequestDto));

            if (response.StatusCode == HttpStatusCode.OK)
            {
                if (RememberMe)
                    await TokenProvider.SetToken(response.Data.AccessToken, response.Data.RefreshToken, response.Data.Expires);
                else
                    await TokenProvider.SetSessionToken(response.Data.AccessToken, response.Data.RefreshToken, response.Data.Expires);

                await TokenProvider.CheckAuthentication(true);

                NavigationManager.NavigateTo("/main");
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
