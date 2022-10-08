using Microsoft.AspNetCore.Components;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Client.Base.Layout;
using StepEbay.Main.Client.Common.DataValidationServices;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Auth;
using StepEbay.PushMessage.Services;
using System.Net;

namespace StepEbay.Main.Client.Base.Components.SignUp
{
    [Layout(typeof(EmptyLayout))]
    public partial class SignUp
    {
        [Inject] IApiService ApiService { get; set; }
        [Inject] IMessageService MessageService { get; set; }
        [Parameter] public EventCallback<bool> OnClose { get; set; }
        private SignUpRequestDto SignUpRequestDto { get; set; } = new();
        private bool ShowPreloader { get; set; } = false;

        private Dictionary<string, List<string>> Errors = new();

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
                    ShowPreloader = false;
                    StateHasChanged();
                }

                Errors = response.Errors;
            }

            if (Errors.Count == 0)
            {
                MessageService.ShowSuccsess("Успіх", "Вам надіслано лист для підтвердження реєстрації!");
                ModalClose();
                return;
            }

            ShowPreloader = false;
            StateHasChanged();
        }

        private Task ModalClose()
        {
            return OnClose.InvokeAsync(false);
        }
    }
}