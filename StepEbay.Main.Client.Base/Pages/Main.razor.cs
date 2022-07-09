using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Common.Providers;
using StepEbay.Main.Client.Common.RestServices;

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
        public bool ShowModal { get; set; } = false;

        private Dictionary<string, List<string>> MessageConfirmReg = new();
        private bool ShowPreloader { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Key))
            {
                ShowPreloader = true;

                var response = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.ConfirmRegistration(Id, Key));

                MessageConfirmReg = response.Errors;

                if (!response.Data.Value)
                    MessageConfirmReg.Add("Помилка активації", new List<string> { "Невірні дані для активації акаунта!" });

                if(MessageConfirmReg.Count == 0)
                    MessageConfirmReg.Add("Успішно", new List<string> { "Акаунт активовано!" });

                Id = null;
                Key = null;

                ShowModal = true;
            }

            ShowPreloader = false;
            StateHasChanged();
        }

        private async Task PlaceBet()
        {
            await ApiService.ExecuteRequest(() => ApiService.ApiMethods.PlaceBet(1));
        }

        private void CloseModal(bool show)
        {
            ShowModal = show;
            StateHasChanged();
        }
    }
}
