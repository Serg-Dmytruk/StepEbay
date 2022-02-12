using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StepEbay.Data;
using StepEbay.Worker.HostedService;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        //option configure
        IConfiguration configuration = hostContext.Configuration;

        //TODO ADD JSON APP SETTINGS
        //conection string configure
        string applicationDbContext = hostContext.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(applicationDbContext));
        services.AddHostedService<ImageCleanerService>();

    }).Build();