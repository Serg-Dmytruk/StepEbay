using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Common.Models.Person;

namespace StepEbay.Main.Api.Common.Services.PersonalAccountServices
{
    public interface IPersonService
    {
        public Task<BoolResult> TryUpdate(int id, PersonUpdateRequestDto personUpdateRequest);
        public Task<ResponseData<PersonResponseDto>> GetPersonToUpdateInCabinet(int id);
    }
}
