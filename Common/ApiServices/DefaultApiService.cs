using Refit;
using StepEbay.Common.Models.RefitModels;
using System.Net;
using System.Text.Json;

namespace StepEbay.Common.ApiServices
{
    public class DefaultApiService<TApiMethods> : IDefaultApiService<TApiMethods>
    {
        public HttpClient HttpClient { get; set; }
        public TApiMethods ApiMethods { get; set; }

        public DefaultApiService(HttpClient httpClient, string connectionString)
        {
            HttpClient = httpClient;
            HttpClient.BaseAddress = new Uri(connectionString);
            ApiMethods = RestService.For<TApiMethods>(HttpClient);
        }

        private async Task<ResponseData<T>> ExecuteInnerRequest<T>(Func<Task<ApiResponse<T>>> request) where T : class
        {
            ApiResponse<T> response = await request.Invoke();
            ResponseData<T> result = new() { StatusCode = response.StatusCode };

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    result.Data = response.Content;
                    break;

                case HttpStatusCode.BadRequest:
                    result.Errors = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(response.Error.Content);
                    break;

            }
            return result;
        }

      
        public async Task<ResponseData<T>> ExecuteRequest<T>(Func<Task<ApiResponse<T>>> request) where T : class
        {
            return await ExecuteInnerRequest(request);
        }

        //TODO autorazition
        //private async Task SetAuthorization()
        //{
        //    string token = null;

        //    await _locker.LockAsync(async () =>
        //    {
        //        token = await _authProvider.GetToken();

        //        if (!string.IsNullOrEmpty(token))
        //        {
        //            long expires = await _authProvider.GetExpires();

        //            if (expires <= DateTime.UtcNow.ToUnix())
        //            {
        //                if (await Refresh(token))
        //                    token = await _authProvider.GetToken();
        //                else
        //                    token = null;
        //            }
        //        }
        //    });

        //    HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //}

        //protected abstract Task<(RefreshTokenData, HttpStatusCode)> Refresh(RefreshTokenData data);

        //private async Task<bool> Refresh(string token)
        //{
        //    string refreshToken = await _authProvider.GetRefreshToken();

        //    if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
        //    {
        //        await _authProvider.RemoveToken();
        //        return false;
        //    }

        //    (RefreshTokenData, HttpStatusCode) response = await Refresh(new RefreshTokenData
        //    {
        //        AccessToken = token,
        //        RefreshToken = refreshToken
        //    });

        //    if (response.Item2 == HttpStatusCode.OK)
        //    {
        //        if (await _authProvider.GetRememberMe())
        //            await _authProvider.SetToken(response.Item1.AccessToken, response.Item1.RefreshToken, response.Item1.Expires);
        //        else
        //            await _authProvider.SetSessionToken(response.Item1.AccessToken, response.Item1.RefreshToken, response.Item1.Expires);

        //        return true;
        //    }

        //    await _authProvider.RemoveToken();
        //    return false;
        //}
    }
}
