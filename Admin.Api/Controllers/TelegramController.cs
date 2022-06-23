using Microsoft.AspNetCore.Mvc;
using StepEbay.Admin.Api.Services.Telegram;
using StepEbay.Admin.Common.Models.Telegram;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Models.Telegram;

namespace StepEbay.Admin.Api.Controllers
{
    [Route("telegram")]
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
        public async Task<BoolResult> AddGroup(string groupTg)
        {
            return await _serviceTg.AddGroup(groupTg);
        }

        /// <summary>
        /// Видаляє телеграм групу за ID
        /// </summary>
        [HttpDelete("group/removeid/{groupTgId}")]
        public async Task<BoolResult> RemoveGroup(int groupTgId)
        {
            return await _serviceTg.RemoveGroup(groupTgId);
        }

        /// <summary>
        /// Видаляє телеграм групу за токену
        /// </summary>
        [HttpDelete("group/removetoken/{groupTg}")]
        public async Task<BoolResult> RemoveGroupToken(string groupTg)
        {
            return await _serviceTg.RemoveGroup(groupTg);
        }

        /// <summary>
        /// Редагує телеграм групу за ID
        /// </summary>
        [HttpPatch("group/updateid/{groupTgId}/{newToken}")]
        public async Task<BoolResult> UpdateGroup(int groupTgId, string newToken)
        {
            return await _serviceTg.UpdateGroup(groupTgId, newToken);
        }

        /// <summary>
        /// Редагує телеграм групу за токеном
        /// </summary>
        [HttpPatch("group/updatetoken/{oldToken}/{newToken}")]
        public async Task<BoolResult> UpdateGroupToken(string oldToken, string newToken)
        {
            return await _serviceTg.UpdateGroup(oldToken, newToken);
        }

        /// <summary>
        /// Повертає усі телеграм групи у вигляді list
        /// </summary>
        [HttpPost("group/all/list")]
        public async Task<List<DeveloperGroup>> GetAllGroup()
        {
            return await _serviceTg.GetAllGroups();
        }
        /// <summary>
        /// Повертає усі телеграм групи у вигляді DTO (Чомусь не працює)
        /// </summary>
        [HttpPost("group/all")]
        public async Task<GroupsResponseDto> GetAllGroupDto()
        {
            List<DeveloperGroup> l = (await _serviceTg.GetAllGroups());
            GroupsResponseDto result = new GroupsResponseDto() { list = l };
            return result;
        }
    }
}
