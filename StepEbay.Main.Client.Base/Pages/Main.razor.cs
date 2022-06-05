using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Common.RestServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Main.Client.Base.Pages
{
    [Authorize]
    [Route("/main")]
    public partial class Main
    {
        [Inject] IApiService ApiService { get; set; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
               
                
            }
        }

        private async Task PlaceBet()
        {
            await ApiService.ExecuteRequest(() => ApiService.ApiMethods.PlaceBet(1));
        }
    }
}
