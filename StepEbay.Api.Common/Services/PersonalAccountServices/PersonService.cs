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
        public async Task<BoolResult> TryUpdate(int id, PersonUpdateRequestDto personUpdateRequest)
        {
            User updateEntity = await _userDbService.Get(id);
            if (!BC.Verify(personUpdateRequest.OldPasswordForConfirm, updateEntity.Password))
                return new BoolResult(false) { ErrorMessage = "Wrong confirmation password" };
            if (personUpdateRequest.Password == null)
            {
                personUpdateRequest.Password = updateEntity.Password;
                personUpdateRequest.PasswordRepeat = updateEntity.Password;
            }

            var validator = new AuthValidator();
            var result = await validator.ValidateAsync(new SignUpRequestDto() { Id = id, NickName = personUpdateRequest.NickName, Email = personUpdateRequest.Email, Password = personUpdateRequest.Password, CopyPassword = personUpdateRequest.PasswordRepeat, FullName = personUpdateRequest.FullName });
            if (!result.IsValid)
                return new BoolResult(false) { ErrorMessage = result.Errors.FirstOrDefault().ToString() };

            bool emailConfirm = true;
            if (personUpdateRequest.Email == updateEntity.Email)
                emailConfirm = false;

            updateEntity.NickName = personUpdateRequest.NickName;
            updateEntity.Email = personUpdateRequest.Email;
            updateEntity.Password = BC.HashPassword(personUpdateRequest.Password);
            updateEntity.FullName = personUpdateRequest.FullName;
            updateEntity.Adress = personUpdateRequest.Adress;
            updateEntity.IsEmailConfirmed = emailConfirm;
            await _userDbService.Update(updateEntity);
            return new BoolResult(true);
        }

        public async Task<ResponseData<PersonResponseDto>> GetPersonToUpdateInCabinet(int id)
        {
            var middlProcessUser = await _userDbService.Get(id);
            return new ResponseData<PersonResponseDto>() { Data = new PersonResponseDto() { Adress = middlProcessUser.Adress, Email = middlProcessUser.Email, Name = middlProcessUser.FullName, NickName = middlProcessUser.NickName } };
        }
    }
}
