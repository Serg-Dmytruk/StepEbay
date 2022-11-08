using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.UserDbServices;
using StepEbay.Data.Models.Users;
using System.Text.RegularExpressions;
using BC = BCrypt.Net.BCrypt;

namespace StepEbay.Admin.Api.Common.Services.User
{
    public class UserService:IUserService
    {
        private readonly IUserDbService _users;

        public UserService(IUserDbService users)
        {
            _users = users;
        }

        public async Task<BoolResult> AddUser(string nickName, string fullName, string email, string adress, string password, bool isEmailConfirmed)
        {
            await _users.Add(new Data.Models.Users.User() { NickName= nickName, FullName= fullName, Email=email, Adress=adress, Password= BC.HashPassword(password), EmailKey= Guid.NewGuid().ToString(), IsEmailConfirmed= isEmailConfirmed, Created=DateTime.UtcNow });
            return new BoolResult(true);
        }

        public async Task<BoolResult> RemoveUser(int id)
        {
            await _users.Remove(_users.Get(id).Result);
            return new BoolResult(true);
        }

        public async Task<BoolResult> UpdateUser(int id, string nickName, string fullName, string email, string adress, string password, bool isEmailConfirmed)
        {
            Data.Models.Users.User updatedUser = await _users.Get(id);

            updatedUser.NickName = nickName;
            updatedUser.FullName = fullName;
            updatedUser.Email = email;
            updatedUser.Adress = adress;
            if (password != string.Empty)
                updatedUser.Password = password;
            updatedUser.IsEmailConfirmed = isEmailConfirmed;
       
            await _users.Update(updatedUser);

            return new BoolResult(true);
        }
    }
}
