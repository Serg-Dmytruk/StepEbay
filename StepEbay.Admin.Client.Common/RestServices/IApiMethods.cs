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

        [Patch("/telegram/group/updateid/{groupTgId}/{newToken}")]
        Task<ApiResponse<BoolResult>> UpdateGroupById(int groupTgId, string newToken);

        [Patch("/telegram/group/updatetoken/{oldToken}/{newToken}")]
        Task<ApiResponse<BoolResult>> UpdateGroupByToken(string oldToken, string newToken);

        [Post("/telegram/group/all")]
        Task<ApiResponse<List<GroupResponseDto>>> GetAllGroup();

        #endregion

        #region product

        [Delete("/product/category/removeid/{categoryId}")]
        Task<ApiResponse<BoolResult>> RemoveCategory(int categoryId);

        [Delete("/product/category/removename/{categoryName}")]
        Task<ApiResponse<BoolResult>> RemoveCategoryName(string categoryName);

        [Patch("/product/category/updateid/{categoryId}/{newCategoryName}")]
        Task<ApiResponse<BoolResult>> UpdateCategory(int categoryId, string newCategoryName);

        [Patch("/product/category/updatename/{oldCategoryName}/{newCategoryName}")]
        Task<ApiResponse<BoolResult>> UpdateCategoryName(string oldCategoryName, string newCategoryName);

        [Post("/product/category/all")]
        Task<ApiResponse<List<CategoryResponseDto>>> GetAllCategoryDto();

        #endregion

        #region user

        [Put("/user/add/{nickName}/{fullName}/{email}/{adress}/{password}/{isEmailConfirmed}")]
        Task<ApiResponse<BoolResult>> AddUser(string nickName, string fullName, string email, string adress, string password, bool isEmailConfirmed);

        [Delete("/user/remove/{id}")]
        Task<ApiResponse<BoolResult>> RemoveUser(int id);

        [Patch("/user/edit/{id}/{nickName}/{fullName}/{email}/{adress}/{password}/{isEmailConfirmed}")]
        Task<ApiResponse<BoolResult>> UpdateUser(int id, string nickName, string fullName, string email, string adress, string password, bool isEmailConfirmed);

        #endregion

        [Post("/auth/signin")]
        Task<ApiResponse<SignInResponseDto>> SignIn(SignInRequestDto request);

        [Post("/auth/refresh")]
        Task<ApiResponse<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto request);


    }
}
