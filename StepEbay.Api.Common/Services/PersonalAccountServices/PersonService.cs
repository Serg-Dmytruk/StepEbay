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

        public async Task<ResponseData> TryUpdate(int id, PersonUpdateRequestDto personUpdateRequest)
        {
            User updateEntity = await _userDbService.Get(id);
            bool hashed = false;

            if (string.IsNullOrEmpty(personUpdateRequest.OldPasswordForConfirm) || !BC.Verify(personUpdateRequest.OldPasswordForConfirm, updateEntity.Password))
                return ResponseData.Fail("password", "Wrong confirmation password");

            if (string.IsNullOrEmpty(personUpdateRequest.Password))
            {
                personUpdateRequest.Password = updateEntity.Password;
                personUpdateRequest.PasswordRepeat = updateEntity.Password;
                hashed = true;
            }

            var validator = new AuthValidator();

            var result = await validator.ValidateAsync(new SignUpRequestDto()
            {
                Id = id,
                NickName = personUpdateRequest.NickName,
                Email = personUpdateRequest.Email,
                Password = personUpdateRequest.Password,
                CopyPassword = personUpdateRequest.PasswordRepeat,
                FullName = personUpdateRequest.FullName
            });

            if (!result.IsValid)
                return ResponseData.Fail("password", result.Errors.First().ToString());

            updateEntity.NickName = personUpdateRequest.NickName;
            updateEntity.Email = personUpdateRequest.Email;
            updateEntity.Password = personUpdateRequest.Password;
            if (hashed)
                updateEntity.Password = BC.HashPassword(updateEntity.Password);
            updateEntity.FullName = personUpdateRequest.FullName;
            updateEntity.Adress = personUpdateRequest.Adress;
            if (personUpdateRequest.Email != updateEntity.Email)
                updateEntity.IsEmailConfirmed = false;

            await _userDbService.Update(updateEntity);

            return ResponseData.Ok();
        }

        public async Task<PersonResponseDto> GetPersonToUpdateInCabinet(int id)
        {
            var user = await _userDbService.Get(id);
            return new PersonResponseDto()
            {
                Adress = user.Adress,
                Email = user.Email,
                Name = user.FullName,
                NickName = user.NickName
            };
        }
    }
}