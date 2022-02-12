using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Main.Client.Common.Pages
{
    [Route("/signup")]
    public partial class Auth
    {
        //TODO [Inject] private ITokenProvider TokenProvider { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
    }
}
