﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Common.Models.Person;

namespace StepEbay.Main.Client.Base.Pages.PersonalAccount
{
    [Route("/person")]
    //[Authorize]
    public partial class PersonalAccount
    {
        [Inject] IApiService ApiService { get; set; }
        private string _message { get; set; }
        private string _niknameValue{get; set;}
        private string _emailValue { get; set; }
        private string _passwordValue { get; set; }
        private string _passwordRepeatValue { get; set; }
        private string _nameValue { get; set; }
        private string _adressValue { get; set; }
        private string _passwordConfirmValue { get; set; }
        protected override async Task OnInitializedAsync()
        {
            
        }
        public async Task Update()
        {
            _message = "Message: ";
            var result=await ApiService.ExecuteRequest(() => ApiService.ApiMethods.TryUpdatePerson(_passwordConfirmValue, _niknameValue,_emailValue,_passwordValue,_passwordRepeatValue,_nameValue,_adressValue));
            if (result.Data.Value)
            {
                _message += "Updated";
            }
            else
            {
                _message += result.Data.ErrorMessage;
            }
            this.StateHasChanged();
        }
    }
}
