using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Common.Models.Bet;
using StepEbay.Main.Common.Models.Person;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Api.Common.Services.PersonalAccountServices
{
    public interface IPersonService
    {
        Task<ResponseData> TryUpdate(int id, PersonUpdateRequestDto personUpdateRequest);
        Task<PersonResponseDto> GetPersonToUpdateInCabinet(int id);
        Task<List<ProductDto>> GetProductsInfo(ProductInfoDto productInfos);
        Task<BoolResult> ToggleFavorite(int productId, int userId);
    }
}
