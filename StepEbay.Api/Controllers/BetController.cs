using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.BetsDbServices;
using StepEbay.Main.Api.Common.Services.BetServices;

namespace StepEbay.Main.Api.Controllers
{
    [Authorize]
    [Route("bet")]
    public class BetController : ControllerBase
    {
        public readonly IBetService _bets;

        public BetController(IBetService bets)
        {
            _bets = bets;
        }

        [HttpPost("place/{lotId}")]
        public async Task<ResponseData> PlaceBet(int lotId)
        {
            int userId = int.Parse(User.Claims.Single(c => c.Type == "nickName").Value);
            return await _bets.PlaceBet(userId, lotId);
        }
    }
}
