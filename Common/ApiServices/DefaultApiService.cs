﻿using Refit;
using StepEbay.Common.Helpers;
using StepEbay.Common.Lockers;
using StepEbay.Common.Models.Auth;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Common.Providers;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace StepEbay.Common.ApiServices
{
    public abstract class DefaultApiService<TApiMethods> : IDefaultApiService<TApiMethods>
    {
        private readonly IDefaultTokenProvider _authProvider;
        private readonly SemaphoreLocker _locker;

        public HttpClient HttpClient { get; set; }
        public TApiMethods ApiMethods { get; set; }

        public DefaultApiService(IDefaultTokenProvider authProvider,
            SemaphoreLocker locker, HttpClient httpClient, string connectionString)
        {
            _authProvider = authProvider;
            _locker = locker;

            HttpClient = httpClient;

            HttpClient.BaseAddress = new Uri(connectionString);
            ApiMethods = RestService.For<TApiMethods>(HttpClient);
        }

        private async Task<ResponseData<T>> ExecuteInnerRequest<T>(Func<Task<ApiResponse<T>>> request) where T : class
        {
            if (_authProvider != null)
                await SetAuthorization();

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

                case HttpStatusCode.Unauthorized:
                    if (_authProvider != null)
                    {
                        await _authProvider.RemoveToken();
                        await _authProvider.CheckAuthentication(false);
                    }
                    break;

            }
            return result;
        }

      
        public async Task<ResponseData<T>> ExecuteRequest<T>(Func<Task<ApiResponse<T>>> request) where T : class
        {
            return await ExecuteInnerRequest(request);
        }

        private async Task SetAuthorization()
        {
            string token = null;

            await _locker.LockAsync(async () =>
            {
                token = await _authProvider.GetToken();

                if (!string.IsNullOrEmpty(token))
                {
                    long expires = await _authProvider.GetExpires();

                    if (expires <= DateTime.UtcNow.ToUnix())
                    {
                        if (await Refresh(token))
                            token = await _authProvider.GetToken();
                        else
                            token = null;
                    }
                }
            });

            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        protected abstract Task<(RefreshTokenData, HttpStatusCode)> Refresh(RefreshTokenData data);

        private async Task<bool> Refresh(string token)
        {
            string refreshToken = await _authProvider.GetRefreshToken();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
            {
                await _authProvider.RemoveToken();
                return false;
            }

            (RefreshTokenData, HttpStatusCode) response = await Refresh(new RefreshTokenData
            {
                AccessToken = token,
                RefreshToken = refreshToken
            });

            if (response.Item2 == HttpStatusCode.OK)
            {
                if (await _authProvider.GetRememberMe())
                    await _authProvider.SetToken(response.Item1.AccessToken, response.Item1.RefreshToken, response.Item1.Expires);
                else
                    await _authProvider.SetSessionToken(response.Item1.AccessToken, response.Item1.RefreshToken, response.Item1.Expires);

                return true;
            }

            await _authProvider.RemoveToken();
            return false;
        }
    }
}
