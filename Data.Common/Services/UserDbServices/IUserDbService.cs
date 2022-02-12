﻿using StepEbay.Data.Common.Services.Default;
using StepEbay.Data.Models.Users;

namespace StepEbay.Data.Common.Services.UserDbServices
{
    public interface IUserDbService : IDefaultDbService<int, User>
    {
    }
}
