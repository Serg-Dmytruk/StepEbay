using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Common.Services.UserDbServices;
using StepEbay.Data.Models.Users;
using StepEbay.Main.Api.Common.Services.DataValidationServices;
using StepEbay.Main.Common.Models.Auth;
using StepEbay.Main.Common.Models.Bet;
using StepEbay.Main.Common.Models.Person;
using StepEbay.Main.Common.Models.Product;
using BC = BCrypt.Net.BCrypt;

namespace StepEbay.Main.Api.Common.Services.PersonalAccountServices
{
    public class PersonService : IPersonService
    {
        IUserDbService _userDbService;
        IProductDbService _productDbService;
        IFavoriteDbService _favoriteDbService;
        public PersonService(IUserDbService userDbService, IProductDbService productDbService, IFavoriteDbService favoriteDbService)
        {
            _userDbService = userDbService;
            _productDbService = productDbService;
            _favoriteDbService = favoriteDbService;
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

        public async Task<List<ProductDto>> GetProductsInfo(ProductInfoDto productInfos)
        {
            return (await _productDbService.GetProductForInfo(productInfos.ProductIds)).Select(x => new ProductDto
            {
                Id = x.Id,
                Image1 = x.Image1,
                Image2 = x.Image2,
                Image3 = x.Image3,
                Title = x.Title,
                Description = x.Description,
                Price = x.Price,
                CategoryId = x.CategoryId,
                StateId = x.ProductStateId,
                OwnerId = x.OwnerId,
                PurchaseTypeId = x.PurchaseTypeId,
                DateCreated = x.DateCreated,
                DateClosed = x.DateClose.Value,
                Rate = x.Rate
            }).ToList();
        }

        public async Task<BoolResult> ToggleFavorite(int productId, int userId)
        {
            return new BoolResult(await _favoriteDbService.ToggleFavorite(productId, userId));
        }

        public async Task<BoolResult> IsFavorite(int productId, int userId)
        {
            return new BoolResult(await _favoriteDbService.IsFavorite(productId, userId));
        }
    }
}