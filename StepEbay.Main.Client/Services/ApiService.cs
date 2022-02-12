using Microsoft.Extensions.Configuration;
using StepEbay.Common.ApiServices;
using StepEbay.Main.Client.Common.RestServices;
using System.Net.Http;

namespace StepEbay.Main.Client.Services
{
    public class ApiService : DefaultApiService<IApiMethods>, IApiService
    {
        public ApiService(HttpClient httpClient, IConfiguration configuration)
            : base(httpClient, configuration.GetConnectionString("Api"))
        {

        }
    }
}
