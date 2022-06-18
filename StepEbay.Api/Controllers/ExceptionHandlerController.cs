using Microsoft.AspNetCore.Mvc;

namespace StepEbay.Main.Api.Controllers
{
    [Route("exception")]
    public class ExceptionHandlerController : ControllerBase
    {

        /// <summary>
        /// Перевірка роботи глобального обробника помилок
        /// </summary>
        /// 
        [HttpGet]
        public void TestException()
        {
            throw new ArgumentException("test api");
        }
    }
}
