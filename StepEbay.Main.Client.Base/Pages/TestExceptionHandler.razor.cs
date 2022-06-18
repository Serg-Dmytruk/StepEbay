using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;


namespace StepEbay.Main.Client.Base.Pages
{
    [Route("/exception")]
    [Authorize(Roles ="admin")]
    public partial class TestExceptionHandler
    {
        private static void TestException()
        {
            throw new ArgumentException("test exception");
        }
    }
}
