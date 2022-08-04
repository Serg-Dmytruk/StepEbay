using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Models.Users;
using StepEbay.Main.Common.Models.Person;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Main.Api.Common.Services.PersonalAccountServices
{
    public interface IPersonService
    {
        public Task<ResponseData<BoolResult>> TryUpdate(int id,string nick, string email, string password, string passwordRepeat, string name, string adress, string passwordConfirm);
        public Task<ResponseData<PersonResponseDto>> GetPersonToUpdateInCabinet(int id);
        //Hardcode next
        public ResponseData<List<User>> GetAllHC();
    }
}
