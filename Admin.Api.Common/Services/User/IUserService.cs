using StepEbay.Common.Models.RefitModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Admin.Api.Common.Services.UserServices
{
    public interface IUserService
    {
        public Task<BoolResult> AddUser(string nickName, string fullName, string email, string adress, string password, bool isEmailConfirmed);
        public Task<BoolResult> RemoveUser(int id);
        public Task<BoolResult> UpdateUser(int id, string nickName, string fullName, string email, string adress, string password, bool isEmailConfirmed);
    }
}
