using Refit;
using StepEbay.Admin.Common.Models.Auth;
using StepEbay.Admin.Common.Models.Products;
using StepEbay.Admin.Common.Models.Telegram;
using StepEbay.Common.Models.RefitModels;

namespace StepEbay.Admin.Client.Common.RestServices
{
    public interface IApiMethods
    {
        #region telegram
        [Put("/telegram/group/add/{groupTg}")]
        Task<ApiResponse<BoolResult>> AddGroup(string groupTg);

        [Put("/product/category/add/{categoryName}")]
        Task<ApiResponse<BoolResult>> AddCategory(string categoryName);

        [Delete("/telegram/group/removeid/{groupTgId}")]
        Task<ApiResponse<BoolResult>> RemoveGroupById(int groupTgId);

        [Delete("/telegram/group/removetoken/{groupTg}")]
        Task<ApiResponse<BoolResult>> RemoveGroupByToken(string groupTg);

        [Delete("/product/category/removeid/{categoryId}")]
        Task<ApiResponse<BoolResult>> RemoveCategory(int categoryId);

        [Delete("/product/category/removename/{categoryName}")]
        Task<ApiResponse<BoolResult>> RemoveCategoryName(string categoryName);

        [Patch("/telegram/group/updateid/{groupTgId}/{newToken}")]
        Task<ApiResponse<BoolResult>> UpdateGroupById(int groupTgId, string newToken);

        [Patch("/telegram/group/updatetoken/{oldToken}/{newToken}")]
        Task<ApiResponse<BoolResult>> UpdateGroupByToken(string oldToken, string newToken);

        [Patch("/product/category/updateid/{categoryId}/{newCategoryName}")]
        Task<ApiResponse<BoolResult>> UpdateCategory(int categoryId, string newCategoryName);

        [Patch("/product/category/updatename/{oldCategoryName}/{newCategoryName}")]
        Task<ApiResponse<BoolResult>> UpdateCategoryName(string oldCategoryName, string newCategoryName);

        [Post("/telegram/group/all")]
        Task<ApiResponse<List<GroupResponseDto>>> GetAllGroup();

        [Post("/product/category/all")]
        Task<ApiResponse<List<CategoryResponseDto>>> GetAllCategoryDto();
        #endregion

        [Post("/auth/signin")]
        Task<ApiResponse<SignInResponseDto>> SignIn(SignInRequestDto request);

        [Post("/auth/refresh")]
        Task<ApiResponse<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto request);
    }
}
