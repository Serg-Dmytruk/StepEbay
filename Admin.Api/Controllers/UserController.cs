using Microsoft.AspNetCore.Mvc;
using StepEbay.Admin.Api.Common.Services.User;
using StepEbay.Common.Models.RefitModels;

namespace StepEbay.Admin.Api.Controllers
{
    [Route("user")]
    public class UserController
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Додає нового користувача у базу данних
        /// </summary>
        [HttpPut("add/{nickName}/{fullName}/{email}/{adress}/{password}/{isEmailConfirmed}")]
        public async Task<BoolResult> AddUser(string nickName, string fullName, string email, string adress, string password, bool isEmailConfirmed)
        {
            return await _userService.AddUser(nickName, fullName, email, adress, password, isEmailConfirmed);
        }

        /// <summary>
        /// Видаляє користувача
        /// </summary>
        [HttpDelete("remove/{id}")]
        public async Task<BoolResult> RemoveUser(int id)
        {
            return await _userService.RemoveUser(id);
        }

        /// <summary>
        /// Редагує користувача
        /// </summary>
        [HttpPatch("edit/{id}/{nickName}/{fullName}/{email}/{adress}/{password}/{isEmailConfirmed}")]
        public async Task<BoolResult> EditUser(int id, string nickName, string fullName, string email, string adress, string password, bool isEmailConfirmed)
        {
            return await _userService.UpdateUser(id, nickName, fullName, email, adress, password, isEmailConfirmed);
        }
    }
}
