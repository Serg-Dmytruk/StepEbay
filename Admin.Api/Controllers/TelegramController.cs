using Microsoft.AspNetCore.Mvc;
using StepEbay.Admin.Api.Services.Telegram;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.TelegramDbServices;
using StepEbay.Data.Models.Telegram;
namespace StepEbay.Admin.Api.Controllers
{
    [Route("error")]
    public class TelegramController : Controller
    {
        private readonly ITelegramService _serviceTg;
        private readonly IDeveloperGroupDbService _groups;
        public TelegramController(ITelegramService serviceTg, IDeveloperGroupDbService groups)
        {
            _serviceTg = serviceTg;
            _groups = groups;
        }
        /// <summary>
        /// Use it for registrate groups where bot will send messages
        /// </summary>
        [HttpPost("group/add")]
        public async Task AddGroup(string groupTg)
        {
            await _serviceTg.SaveGroup(groupTg, _groups);
        }
        /// <summary>
        /// Remove group by id
        /// </summary>
        [HttpPost("group/removeid")]
        public async Task RemoveGroup(int groupTgId)
        {
            await _serviceTg.RemoveGroup(groupTgId,_groups);
        }
        /// <summary>
        /// Remove group by token
        /// </summary>
        [HttpDelete("group/removetoken")]
        public async Task RemoveGroupToken(string groupTg)
        {
            await _serviceTg.RemoveGroup(groupTg, _groups);
        }

        /// <summary>
        /// Update group by id
        /// </summary>
        [HttpPatch("group/updateid")]
        public async Task UpdateGroup(int groupTgId, string newToken)
        {
            await _serviceTg.UpdateGroup(groupTgId, newToken, _groups);
        }
        /// <summary>
        /// Update group by token
        /// </summary>
        [HttpPost("group/updatetoken")]
        public async Task UpdateGroupToken(string oldToken,string newToken)
        {
            await _serviceTg.UpdateGroup(oldToken, newToken, _groups);
        }

        /// <summary>
        /// Return all groups
        /// </summary>
        [HttpPost("group/all")]
        public async Task<List<DeveloperGroup>> GetAllGroup()
        {
            return await _groups.GetAll();
        }
    }
}
