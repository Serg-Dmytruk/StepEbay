using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace StepEbay.Admin.Client.Base.Pages
{
    [Route("/main")]
    [Authorize]
    public partial class Main
    {
    }
}
