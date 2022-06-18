using Serilog.Core;
using Serilog.Events;
using Serilog.HttpLoging.HttpClients;
using System.Reflection;

namespace Serilog.HttpLoging.Helpers
{
    public static class SerilogHelper
    {
        public static Logger Configure(string connection)
        {
            return new LoggerConfiguration()
                .WriteTo.Console(LogEventLevel.Warning)
                .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "log.txt"),
                    LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
                 .WriteTo.Http(connection, restrictedToMinimumLevel: LogEventLevel.Error,
                    httpClient: new SerilogHttpClient(Assembly.GetCallingAssembly().GetName().Name),
                    queueLimitBytes: null)
                .CreateLogger();
        }
    }
}
