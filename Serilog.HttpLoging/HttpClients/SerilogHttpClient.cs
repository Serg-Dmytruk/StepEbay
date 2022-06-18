using Microsoft.Extensions.Configuration;
using Serilog.Sinks.Http;


namespace Serilog.HttpLoging.HttpClients
{
    public class SerilogHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient = new();

        public SerilogHttpClient(string projectName)
        {
            _httpClient.DefaultRequestHeaders.Add("name", projectName);
        }

        public void Configure(IConfiguration configuration)
        {

        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, Stream contentStream)
        {
            using var content = new StreamContent(contentStream);
            content.Headers.Add("Content-Type", "application/json");

            return await _httpClient.PostAsync(requestUri, content);
        }
    }
}
