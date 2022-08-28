using Refit;
using StepEbay.Admin.Common.Models.Products;
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

        [Post("/product/list")]
        Task<ApiResponse<PaginatedList<ProductDto>>> Getproducts();

        [Post("/product/categories")]
        Task<ApiResponse<List<CategoryDto>>> GetCategories();

        [Post("/product/all")]
        Task<ApiResponse<PaginatedList<ProductDto>>> GetProducts(int page);

        [Post("/product/filtered")]
        Task<ApiResponse<PaginatedList<ProductDto>>> GetProductsWithFilters(int[] categoryIds, int minSum, int maxSum, int stateId);

        [Post("/person/update/{person}")]
        Task<ApiResponse<BoolResult>> TryUpdate(PersonUpdateRequestDto person);

        [Post("/person/get")]
        Task<ApiResponse<PersonResponseDto>> GetPersonToUpdateInCabinet();

        [Post("/product/add/{product}")]
        Task<ApiResponse<BoolResult>> AddProduct(ProductDto product);

        [Get("/product/state")]
        Task<ApiResponse<List<StateDto>>> GetAllStates();

        [Get("/product/type")]
        Task<ApiResponse<List<PurchaseTypeResponseDto>>> GetAllPurchaseTypes();
    }
}

