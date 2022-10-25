﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Common.Storages;
using StepEbay.Main.Client.Common.DataValidationServices;
using StepEbay.Main.Client.Common.Options;
using StepEbay.Main.Client.Common.Providers;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Auth;
using StepEbay.PushMessage.Services;
using System.Net;

namespace StepEbay.Main.Client.Base.Components.Auth
{

    public partial class SignModal
    {
        [Inject] private ITokenProvider TokenProvider { get; set; }
        [Inject] IApiService ApiService { get; set; }
        [Inject] private IOptions<AccountOptions> AccountOptions { get; set; }
        [Inject] private LocalStorage LocalStorage { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] IMessageService MessageService { get; set; }

        [Parameter] public EventCallback<bool> OnClose { get; set; }
        private SignInRequestDto SignInRequestDto { get; set; } = new();
        private SignUpRequestDto SignUpRequestDto { get; set; } = new();
        private Dictionary<string, List<string>> Errors = new();
        private bool ShowPreloader { get; set; } = true;
        private bool RememberMe { get; set; }
        private bool IsMainMenu { get; set; } = true;
        private bool IsLogIn { get; set; }
        private bool IsRegistration { get; set; }

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

                await LocalStorage.SetLocal("username", SignInRequestDto.NickName);

                await TokenProvider.CheckAuthentication(true);

                await ModalClose();
                MessageService.ShowSuccsess("Успіх", "Авторизовано");
                return;
            }

            _errors = response.Errors;

            ShowPreloader = false;
            StateHasChanged();
        }
        private async Task SignInRequestAdmin()
        {
            SignInRequestDto = new()
            {
                NickName = AccountOptions.Value.AdminLogin,
                Password = AccountOptions.Value.AdminPassword
            };

            await SignInRequest();
        }

        private async Task SignInRequestUser()
        {
            SignInRequestDto = new()
            {
                NickName = AccountOptions.Value.UserLogin,
                Password = AccountOptions.Value.UserPassword
            };

            await SignInRequest();
        }

        private async Task SignUpRequest()
        {
            Errors.Clear();
            ShowPreloader = true;
            var validator = new AuthValidator();
            var result = await validator.ValidateAsync(SignUpRequestDto);

            if (!result.IsValid)
            {
                var list = new List<string>();
                result.Errors.ForEach(error => list.Add(error.ToString()));
                Errors.Add("Registration", list);
            }
            else
            {
                ResponseData<SignInResponseDto> response = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.SignUp(SignUpRequestDto));

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    ShowPreloader = false;
                    StateHasChanged();
                }

                Errors = response.Errors;
            }

            if (Errors.Count == 0)
            {
                MessageService.ShowSuccsess("Успіх", "Вам надіслано лист для підтвердження реєстрації!");
                await ModalClose();
                return;
            }

            ShowPreloader = false;
            StateHasChanged();
        }

        private Task ModalClose()
        {
            return OnClose.InvokeAsync(false);
        }

        private void ModalBack()
        {
            IsMainMenu = true;
            IsRegistration = false;
            IsLogIn = false;
        }

        private void IsSignUp()
        {
            IsMainMenu = false;
            IsRegistration = true;
            IsLogIn = false;
        }

        private void IsSignIn()
        {
            IsMainMenu = false;
            IsLogIn = true;
            IsRegistration = false;
        }
    }
}