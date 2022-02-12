using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Base.Layout;
using StepEbay.Main.Common.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Main.Client.Base.Pages
{
    [Route("/signup")]
    [Layout(typeof(EmptyLayout))]
    public partial class Auth
    {
        //TODO [Inject] private ITokenProvider TokenProvider { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }

        private SignInRequestDto _signInRequestDto { get; set; }
        private bool _showPreloader { get; set; }
        private bool _rememberMe { get; set; }

        private async Task SigninRequest()
        {
            
        }
    }
}
