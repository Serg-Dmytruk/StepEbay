﻿using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Users;

namespace StepEbay.Data.Common.Services.UserDbServices
{
    public interface IUserDbService : IDefaultDbService<int, User>
    {
        Task<bool> AnyByNickName(string nickName);
        Task<bool> AnyByEmail(string nickName);
        Task<User> GetUserByNickName(string nickName);
        Task<bool> ConfirmEmail(int id, string key);
    }
}
