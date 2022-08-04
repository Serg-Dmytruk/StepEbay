using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.UserDbServices;
using StepEbay.Data.Models.Users;
using StepEbay.Main.Api.Common.Services.DataValidationServices;
using StepEbay.Main.Common.Models.Auth;
using StepEbay.Main.Common.Models.Person;
using BC = BCrypt.Net.BCrypt;

namespace StepEbay.Main.Api.Common.Services.PersonalAccountServices
{
    public class PersonService : IPersonService
    {
        IUserDbService _userDbService;
        public PersonService(IUserDbService userDbService)
        {
            _userDbService = userDbService;
        }
        public async Task<ResponseData<BoolResult>> TryUpdate(int id, string nick, string email, string password, string passwordRepeat, string name, string adress, string passwordConfirm)
        {
            User updateEntity= await _userDbService.Get(id);
            if (passwordConfirm == BC.HashPassword(updateEntity.Password)) 
            {
                var validator = new AuthValidator();
                var result = await validator.ValidateAsync(new SignUpRequestDto() { Id = id, NickName = nick, Email = email, Password = password, CopyPassword = passwordRepeat, FullName = name });
                if (result.IsValid)
                {
                    bool emailConfirm = true;

                    if (email == updateEntity.Email)
                    {
                        emailConfirm = false;
                    }
                    updateEntity.NickName = nick;
                    updateEntity.Email = email;
                    updateEntity.Password = BC.HashPassword(password);
                    updateEntity.FullName = name;
                    updateEntity.Adress = adress;
                    updateEntity.IsEmailConfirmed = emailConfirm;

                    await _userDbService.Update(updateEntity);
                    return new ResponseData<BoolResult>() { Data = new BoolResult(true) };
                }
                else
                {
                    return new ResponseData<BoolResult>() { Data = new BoolResult(false, result.Errors.First().ErrorMessage) };
                }
            }
            else
            {
                return new ResponseData<BoolResult>() { Data = new BoolResult(false, "Wrong confirmation password") };
            }
        }
        public async Task<ResponseData<PersonResponseDto>> GetPersonToUpdateInCabinet(int id)
        {
            var tmpUser=await _userDbService.Get(id);
            return new ResponseData<PersonResponseDto>() { Data = new PersonResponseDto() { Adress=tmpUser.Adress, Email=tmpUser.Email, Name=tmpUser.FullName, NickName=tmpUser.NickName} };
        }
        
        //Hardcode next
        public ResponseData<List<User>> GetAllHC()
        {
            return new ResponseData<List<User>> { Data= _userDbService.List().Result } ;
        }
    }
}
