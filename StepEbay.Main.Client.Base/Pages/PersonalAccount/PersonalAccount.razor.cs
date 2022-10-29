using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Common.ClientsHub;
using StepEbay.Main.Client.Common.Providers;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Person;
using StepEbay.PushMessage.Services;
using System.Net;

namespace StepEbay.Main.Client.Base.Pages.PersonalAccount
{
    [Authorize]
    public partial class PersonalAccount
    {
        [Inject] IApiService _apiService { get; set; }
        [Inject] IMessageService _messageService { get; set; }
        [Inject] private ITokenProvider TokenProvider { get; set; }
        [Inject] private HubClient HubClient { get; set; }

        private PersonUpdateRequestDto _request { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _request = new();
            PersonResponseDto person = (await _apiService.ExecuteRequest(() => _apiService.ApiMethods.GetPersonToUpdateInCabinet())).Data;
            _request.NickName = person.NickName;
            _request.Email = person.Email;
            _request.FullName = person.Name;
            _request.Adress = person.Adress;

            StateHasChanged();
        }

        private async Task Logout()
        {
            await TokenProvider.RemoveToken();
            await TokenProvider.CheckAuthentication(false);

            await HubClient.Stop();
        }

        protected async Task UpdatePerson()
        {
            var result = await _apiService.ExecuteRequest(() => _apiService.ApiMethods.TryUpdate(_request));

            if (result.StatusCode != HttpStatusCode.OK)
                _messageService.ShowError("Помилка", result.Errors.First().Value.First());
            else
            {
                _messageService.ShowSuccsess("Успіх!", "Акаунт оновлено");
                _request = new();
                StateHasChanged();
            }
        }
    }
}
