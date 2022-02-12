using Microsoft.AspNetCore.Components;
using StepEbay.Main.Client.Base.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Main.Client.Base.Pages
{
    [Route("/signup")]
    [Layout(typeof(EmptyLayout))]
    public partial class SignUp
    {
      
        [Inject] private NavigationManager NavigationManager { get; set; }
    }
}
