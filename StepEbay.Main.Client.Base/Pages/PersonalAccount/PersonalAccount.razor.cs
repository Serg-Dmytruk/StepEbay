using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Person;
using System.Net;

namespace StepEbay.Main.Client.Base.Pages.PersonalAccount
{
    [Route("/person")]
    [Authorize]
    public partial class PersonalAccount
    {
        [Inject] IApiService _apiService { get; set; }
        private string _message { get; set; }
        private string _niknameValue { get; set; }
        private string _emailValue { get; set; }
        private string _passwordValue { get; set; }
        private string _passwordRepeatValue { get; set; }
        private string _nameValue { get; set; }
        private string _adressValue { get; set; }
        private string _passwordConfirmValue { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PersonResponseDto person = (await _apiService.ExecuteRequest(() => _apiService.ApiMethods.GetPersonToUpdateInCabinet())).Data;
            _niknameValue = person.NickName;
            _emailValue = person.Email;
            _nameValue = person.Name;
            _adressValue = person.Adress;

            StateHasChanged();
        }

        //TODO перобити на бінди
        protected async Task UpdatePerson()
        {
            var request = new PersonUpdateRequestDto() { NickName = _niknameValue, Email = _emailValue, Password = _passwordValue, PasswordRepeat = _passwordRepeatValue, FullName = _nameValue, Adress = _adressValue, OldPasswordForConfirm = _passwordConfirmValue };
            var result = await _apiService.ExecuteRequest(() => _apiService.ApiMethods.TryUpdate(request));

            if (result.StatusCode != HttpStatusCode.OK)
                _message = result.Errors.First().Value.First();
            else
                _message = "Updated";

            StateHasChanged();
        }
    }
}
