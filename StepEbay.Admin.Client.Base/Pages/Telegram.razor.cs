using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using StepEbay.Admin.Client.Common.RestServices;

namespace StepEbay.Admin.Client.Base.Pages
{
    [Route("/telegram")]
    //[Authorize(Roles = "admin, manager")]
    public partial class Telegram
    {
        private string _tokenToAdd;
        private string _tokenToRemove;
        private int? _idToRemove;

        private int? _idToUpdate;
        private string _tokenToUpdate;
        private string _valueToUpdate1;
        private string _valueToUpdate2;
        [Inject] IApiService _service { get; set; }
        
        public async Task AddGroup()
        {
            await _service.ExecuteRequest(()=>_service.ApiMethods.AddGroup(_tokenToAdd));
            _tokenToAdd = null;
            this.StateHasChanged();
        }
        public async Task RemoveGroupById()
        {
            await _service.ExecuteRequest(() => _service.ApiMethods.RemoveGroupById(_idToRemove.Value));
            _idToRemove = null;
            this.StateHasChanged();
        }
        public async Task RemoveGroupByToken()
        {
            await _service.ExecuteRequest(() => _service.ApiMethods.RemoveGroupByToken(_tokenToRemove));
            _tokenToRemove = null;
            this.StateHasChanged();
        }
        public async Task UpdateGroupById()
        {
            await _service.ExecuteRequest(() => _service.ApiMethods.UpdateGroupById(_idToUpdate.Value,_valueToUpdate1));
            _idToUpdate = null;
            _valueToUpdate1=null;
            this.StateHasChanged();
        }
        public async Task UpdateGroupByToken()
        {
            await _service.ExecuteRequest(() => _service.ApiMethods.UpdateGroupByToken(_tokenToUpdate, _valueToUpdate2));
            _tokenToUpdate = null;
            _valueToUpdate2 = null;
            this.StateHasChanged();
        }
    }
}
