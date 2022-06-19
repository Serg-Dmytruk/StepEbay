using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using StepEbay.Admin.Client.Common.RestServices;

namespace StepEbay.Admin.Client.Base.Pages
{
    [Route("/telegram")]
    //[Authorize(Roles = "admin, manager")]
    public partial class Telegram
    {
        private string? _inputValue;
        [Inject] IApiService _service { get; set; }
        
        public async Task UpdateToken()
        {
            await _service.ExecuteRequest(()=>_service.ApiMethods.AddGroup(_inputValue));
        }
    }
}
