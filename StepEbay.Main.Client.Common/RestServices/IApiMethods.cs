using Refit;
using StepEbay.Common.Models.Pagination;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Main.Common.Models.Auth;
using StepEbay.Main.Common.Models.Person;
using StepEbay.Main.Common.Models.Product;

namespace StepEbay.Main.Client.Common.RestServices
{
    public interface IApiMethods
    {
        [Post("/auth/signin")]
        Task<ApiResponse<SignInResponseDto>> SignIn(SignInRequestDto request);

        [Post("/auth/signup")]
        Task<ApiResponse<SignInResponseDto>> SignUp(SignUpRequestDto request);

        [Post("/auth/refresh")]
        Task<ApiResponse<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto request);

        [Post("/bet/place/{lotId}")]
        Task<ApiResponse<BoolResult>> PlaceBet(int lotId);

        [Get("/email/confirm/{id}/{key}")]
        Task<ApiResponse<BoolResult>> ConfirmRegistration(string id, string key);

        [Get("/product/categories")]
        Task<ApiResponse<List<CategoryDto>>> GetCategories();

        [Post("/product/filtered")]
        Task<ApiResponse<PaginatedList<ProductDto>>> GetProductsWithFilters(ProductFilterInfo info, int page);

        [Get("/product/states")]
        Task<ApiResponse<List<ProductStateDto>>> GetProductStates();

        [Post("/person/update")]
        Task<ApiResponse<ResponseData>> TryUpdate(PersonUpdateRequestDto person);

        [Get("/person/get")]
        Task<ApiResponse<PersonResponseDto>> GetPersonToUpdateInCabinet();

        [Post("/product/add")]
        Task<ApiResponse<ResponseData>> AddProduct(ProductDto product);

        [Get("/product/type")]
        Task<ApiResponse<List<PurchaseTypeResponseDto>>> GetAllPurchaseTypes();

        [Get("/product/personal/products")]
        Task<ApiResponse<PaginatedList<ProductDto>>> GetPersonalProduct(int page, bool active, bool closed);

        [Get("/product/{id}")]
        Task<ApiResponse<ProductDto>> GetProduct(int id);
    }
}

