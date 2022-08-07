using StepEbay.Data.Common.Services.UserDbServices;

namespace StepEbay.Main.Api.Common.Services.PersonalAccountServices
{
    public class PersonService : IPersonService
    {
        IUserDbService _userDbService;
        public PersonService(IUserDbService userDbService)
        {
            _userDbService= userDbService;
        }
    }
}
