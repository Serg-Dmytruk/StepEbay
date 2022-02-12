using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Main.Client.Common.Pages
{
    [Route("/signup")]
    public partial class SignUp
    {
      
        [Inject] private NavigationManager NavigationManager { get; set; }
    }
}
