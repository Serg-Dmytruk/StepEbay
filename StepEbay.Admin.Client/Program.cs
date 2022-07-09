using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.HttpLoging.Helpers;
using System;

namespace StepEbay.Admin.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = ConfigurationHelper.Get(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
            Log.Logger = SerilogHelper.Configure($"{configuration.GetConnectionString("ControlPanel")}/exception/log");

            try
            {
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}
