﻿using Microsoft.AspNetCore.Components;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Client.Base.Layout;
using StepEbay.Main.Client.Common.DataValidationServices;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Auth;
using System.Net;

namespace StepEbay.Main.Client.Base.Components.SignUp
{
    [Layout(typeof(EmptyLayout))]
    public partial class SignUp
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] IApiService ApiService { get; set; }
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        private SignUpRequestDto SignUpRequestDto { get; set; } = new();
        private bool ShowPreloader { get; set; } = false;

        private Dictionary<string, List<string>> Errors = new();

        private bool ShowModal { get; set; } = false;

        private readonly List<string> RegMess = new() { "Вам надіслано лист для підтвердження реєстрації!" };

        protected void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                ShowPreloader = false;
                StateHasChanged();
            }
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
                    ShowModal = true;
                    ShowPreloader = false;
                    StateHasChanged();
                    return;
                }

                Errors = response.Errors;
            }

            if (Errors.Count > 0)
                ShowModal = true;

            ShowPreloader = false;
            StateHasChanged();
        }

        private Task ModalClose()
        {
            return OnClose.InvokeAsync(false);
        }

        private void CloseModal(bool show)
        {
            ShowModal = show;
            StateHasChanged();
        }
    }
}