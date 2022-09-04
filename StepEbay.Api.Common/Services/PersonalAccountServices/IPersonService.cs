using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Common.Models.Person;

namespace StepEbay.Main.Api.Common.Services.PersonalAccountServices
{
    public interface IPersonService
    {
        public Task<ResponseData> TryUpdate(int id, PersonUpdateRequestDto personUpdateRequest);
        public Task<PersonResponseDto> GetPersonToUpdateInCabinet(int id);
    }
}
