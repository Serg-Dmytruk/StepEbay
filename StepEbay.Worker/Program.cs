using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.HttpLoging.Helpers;
using StepEbay.Data;
using StepEbay.Worker.Services;

var environment = /*args.Length == 0 ?*/ "Development" /*: args[0]*/;
Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
Environment.SetEnvironmentVariable("STEP_EBAY_ENVIRONMENT", environment);
var configuration = ConfigurationHelper.Get(environment);
Log.Logger = SerilogHelper.Configure($"{configuration.GetConnectionString("ControlPanel")}/error/handler");

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;

    
        //db
        string applicationDbContext = hostContext.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(applicationDbContext));

        //services
        services.AddHubClients();
        services.AddHosdedServices();

    }).Build();