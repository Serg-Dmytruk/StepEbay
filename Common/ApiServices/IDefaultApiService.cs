using Refit;
using StepEbay.Common.Models.RefitModels;

namespace StepEbay.Common.Client.ApiServices
{
    public interface IDefaultApiService<TApiMethods>
    {
        public TApiMethods ApiMethods { get; set; }
        public Task<ResponseData<T>> ExecuteRequest<T>(Func<Task<ApiResponse<T>>> func) where T : class;

    }
}
