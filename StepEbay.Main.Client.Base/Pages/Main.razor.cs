using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Common.RestServices;
using Microsoft.Extensions.Configuration;

namespace StepEbay.Main.Client.Base.Pages
{
    [Route("/")]
    [Route("main")]
    [Route("main/{id}/{key}")]
    public partial class Main
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public string Key { get; set; }
        [Inject] IApiService ApiService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IConfiguration Configuration { get; set; }
        private string ApiConnection { get; set; }
        public bool ShowModal { get; set; } = false;

        private Dictionary<string, List<string>> MessageConfirmReg = new();

        private bool ShowPreloader { get; set; } = true;

        protected override void OnInitialized()
        {
            ApiConnection = Configuration.GetConnectionString("Api");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            ShowPreloader = true;

            if (!string.IsNullOrEmpty(Id) && !string.IsNullOrEmpty(Key))
            {
                var response = await ApiService.ExecuteRequest(() => ApiService.ApiMethods.ConfirmRegistration(Id, Key));

                MessageConfirmReg = response.Errors;

                if (!response.Data.Value)
                    MessageConfirmReg.Add("Помилка активації", new List<string> { "Невірні дані для активації акаунта!" });

                if (MessageConfirmReg.Count == 0)
                    MessageConfirmReg.Add("Успішно", new List<string> { "Акаунт активовано!" });

                ShowModal = true;
                NavigationManager.NavigateTo("/");
            }

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
