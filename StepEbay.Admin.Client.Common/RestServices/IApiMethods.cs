using Refit;
using StepEbay.Admin.Common.Models.Auth;
using StepEbay.Admin.Common.Models.Telegram;
using StepEbay.Common.Models.RefitModels;

namespace StepEbay.Admin.Client.Common.RestServices
{
    public interface IApiMethods
    {
        #region telegram
        [Put("/telegram/group/add/{groupTg}")]
        Task<ApiResponse<BoolResult>> AddGroup(string groupTg);

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

        [Post("/auth/signin")]
        Task<ApiResponse<SignInResponseDto>> SignIn(SignInRequestDto request);

        [Post("/auth/refresh")]
        Task<ApiResponse<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto request);
    }
}
