using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace StepEbay.Admin.Client.Base.Pages
{
    [Route("/telegram")]
    [Authorize(Roles = "admin, manager")]
    public partial class Telegram
    {
    }
}
