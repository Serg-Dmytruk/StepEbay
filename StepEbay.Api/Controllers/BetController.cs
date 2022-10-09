using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.BetsDbServices;
using StepEbay.Main.Api.Common.Services.BetServices;
using System.Security.Claims;

namespace StepEbay.Main.Api.Controllers
{
    [Authorize]
    [Route("bet")]
    public class BetController : ControllerBase
    {
        private readonly IBetService _bets;

        public BetController(IBetService bets)
        {
            _bets = bets;
        }

        [HttpPost("place/{lotid}")]
        public async Task<ResponseData> PlaceBet(int lotid)
        {
            int userId = int.Parse(User.Claims.Single(c => c.Type == ClaimTypes.Name).Value);
            return await _bets.PlaceBet(userId, lotid);
        }
    }
}
