using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using StepEbay.Admin.Client.Common.RestServices;

namespace StepEbay.Admin.Client.Base.Pages
{
    [Route("/telegram")]
    [Authorize(Roles = "admin")]
    public partial class User
    {
        [Inject] IApiService _service { get; set; }

        private string _nickName1= string.Empty;
        private string _fullName1= string.Empty;
        private string _email1= string.Empty;
        private string _adress1 = string.Empty;
        private string _password1 = string.Empty;
        private bool _isEmailConfirmed1=false;

        private int _id2 = 0;

        private int _id3 = 0;
        private string _nickName3 = string.Empty;
        private string _fullName3 = string.Empty;
        private string _email3 = string.Empty;
        private string _adress3 = string.Empty;
        private string _password3 = string.Empty;
        private bool _isEmailConfirmed3 = false;

        public async Task AddUser()
        {
            await _service.ExecuteRequest(() => _service.ApiMethods.AddUser(_nickName1, _fullName1, _email1, _adress1, _password1, _isEmailConfirmed1));
            _nickName1 = string.Empty;
            _fullName1 = string.Empty;
            _email1 = string.Empty;
            _adress1 = string.Empty;
            _password1 = string.Empty;
            _isEmailConfirmed1 = false;
            this.StateHasChanged();
        }
        public async Task RemoveUser()
        {
            await _service.ExecuteRequest(() => _service.ApiMethods.RemoveUser(_id2));
            _id2 = 0;
            this.StateHasChanged();
        }
        public async Task UpdateUser()
        {
            await _service.ExecuteRequest(() => _service.ApiMethods.UpdateUser(_id3,_nickName3, _fullName3, _email3, _adress3, _password3, _isEmailConfirmed3));
            _nickName3 = string.Empty;
            _fullName3 = string.Empty;
            _email3 = string.Empty;
            _adress3 = string.Empty;
            _password3 = string.Empty;
            _isEmailConfirmed3 = false;
            this.StateHasChanged();
        }
    }
}
