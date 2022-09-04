using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Person;
using StepEbay.PushMessage.Services;
using System.Net;

namespace StepEbay.Main.Client.Base.Pages.PersonalAccount
{
    [Route("/person")]
    [Authorize]
    public partial class PersonalAccount
    {
        [Inject] IApiService _apiService { get; set; }
        [Inject] IMessageService _messageService { get; set; }
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
        protected async Task UpdatePerson()
        {
            var result = await _apiService.ExecuteRequest(() => _apiService.ApiMethods.TryUpdate(_request));

            if (result.StatusCode != HttpStatusCode.OK)
                _messageService.ShowError("Помилка", result.Errors.First().Value.First());
            else
            {
                _messageService.ShowSuccsess("Успіх!", "Товар додано");
                _request = new();
                StateHasChanged();
            }
        }
    }
}
