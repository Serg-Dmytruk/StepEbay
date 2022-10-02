using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Common.Providers;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Product;
using StepEbay.PushMessage.Services;

namespace StepEbay.Main.Client.Base.Pages
{
    [Route("/")]
    [Route("main")]
    [Route("main/{id}/{key}")]
    public partial class Main
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public string Key { get; set; }
        [Inject] private ITokenProvider TokenProvider { get; set; }
        [Inject] IApiService ApiService { get; set; }
        [Inject] IMessageService MessageService { get; set; }
   
        public bool ShowModal { get; set; } = false;
        public bool ShowSignInModal { get; set; } = false;
        public bool ShowSignUpModal { get; set; } = false;

        private Dictionary<string, List<string>> MessageConfirmReg = new();
        private List<CategoryDto> _categories = new List<CategoryDto>();

        private bool ShowPreloader { get; set; } = true;

        protected async Task GetCategories()
        {
            var responce = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.GetCategories());
            _categories = responce.Data;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            ShowPreloader = true;

            if (firstRender)
                await GetCategories();


            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Key))
            {
                var response = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.ConfirmRegistration(Id, Key));

                MessageConfirmReg = response.Errors;

                if (!response.Data.Value)
                    MessageConfirmReg.Add("Помилка активації", new List<string> { "Невірні дані для активації акаунта!" });

                if (MessageConfirmReg.Count == 0)
                    MessageConfirmReg.Add("Успішно", new List<string> { "Акаунт активовано!" });

                ShowModal = true;
            }

            ShowPreloader = false;
            StateHasChanged();
        }

        private async Task PlaceBet()
        {
            await ApiService.ExecuteRequest(() => ApiService.ApiMethods.PlaceBet(1));
        }

        private void Show()
        {
            MessageService.ShowSuccsess("Успішно", "робіть");
            MessageService.ShowWarning("Увага", "робіть");
            MessageService.ShowError("Помилка", "робіть");
            MessageService.ShowInfo("Інфо", "робіть");
        }

        private void CloseModal(bool show)
        {
            ShowModal = show;
            StateHasChanged();
        }

        private void ShowSignIn() => ShowSignInModal = true;

        private void CloseSignInModal(bool show)
        {
            ShowSignInModal = false;
            StateHasChanged();
        }

        private void ShowSignUp()
        {
            ShowSignInModal = false;
            ShowSignUpModal = true;
        }
    }
}
