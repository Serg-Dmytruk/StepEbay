using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace StepEbay.Admin.Client.Base.Pages
{
    [Route("/exception")]
    [Authorize(Roles = "admin")]
    public partial class ExceptionTest
    {
        private static void TestException()
        {
            throw new ArgumentException("test exception");
        }
    }
}
