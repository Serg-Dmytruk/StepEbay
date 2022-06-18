using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace StepEbay.Admin.Client.Base.Pages
{
    [Route("/telegram")]
    [Authorize]
    public partial class Telegram
    {
    }
}
