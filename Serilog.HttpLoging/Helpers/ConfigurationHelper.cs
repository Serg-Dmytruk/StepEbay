using Microsoft.Extensions.Configuration;

namespace Serilog.HttpLoging.Helpers
{
    public static class ConfigurationHelper
    {
        public static IConfiguration Get(string environment)
        {
            return new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").AddJsonFile($"appsettings.{environment}.json").Build();
        }
    }
}
