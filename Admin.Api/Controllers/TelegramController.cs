using Microsoft.AspNetCore.Mvc;
using StepEbay.Admin.Api.Services.Telegram;
using StepEbay.Data.Models.Telegram;

namespace StepEbay.Admin.Api.Controllers
{
    [Route("error")]
    public class TelegramController : ControllerBase
    {
        private readonly ITelegramService _serviceTg;

        public TelegramController(ITelegramService serviceTg)
        {
            _serviceTg = serviceTg;
        }

        /// <summary>
        /// Додає токен телеграм групи у базу данних
        /// </summary>
        [HttpPut("group/add/{groupTg}")]
        public async Task AddGroup(string groupTg)
        {
            await _serviceTg.SaveGroup(groupTg);
        }

        /// <summary>
        /// Видаляє телеграм групу за ID
        /// </summary>
        [HttpDelete("group/removeid/{groupTgId}")]
        public async Task RemoveGroup(int groupTgId)
        {
            await _serviceTg.RemoveGroup(groupTgId);
        }

        /// <summary>
        /// Видаляє телеграм групу за токену
        /// </summary>
        [HttpDelete("group/removetoken/{groupTg}")]
        public async Task RemoveGroupToken(string groupTg)
        {
            await _serviceTg.RemoveGroup(groupTg);
        }

        /// <summary>
        /// Редагує телеграм групу за ID
        /// </summary>
        [HttpPatch("group/updateid/{groupTgId}/{newToken}")]
        public async Task UpdateGroup(int groupTgId, string newToken)
        {
            await _serviceTg.UpdateGroup(groupTgId, newToken);
        }

        /// <summary>
        /// Редагує телеграм групу за токеном
        /// </summary>
        [HttpPatch("group/updatetoken/{oldToken}/{newToken}")]
        public async Task UpdateGroupToken(string oldToken,string newToken)
        {
            await _serviceTg.UpdateGroup(oldToken, newToken);
        }

        /// <summary>
        /// Повертає усі телеграм групи
        /// </summary>
        [HttpPost("group/all")]
        public async Task<List<DeveloperGroup>> GetAllGroup()
        {
            return await _serviceTg.GetAllGroups();
        }
    }
}
