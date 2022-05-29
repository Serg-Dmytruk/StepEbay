using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StepEbay.Main.Api.Controllers
{
    [Authorize]
    [Route("bet")]
    public class BetController : ControllerBase
    {

        [HttpPost("place/{lotId}")]

        public async Task PlaceBet(int lotId)
        {
            var t = User.Claims.Single(c => c.Type == "nickName").Value;
            
        }
    }
}
