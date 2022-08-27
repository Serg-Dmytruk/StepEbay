using Microsoft.Extensions.DependencyInjection;
using StepEbay.Worker.ClientHubs;
using StepEbay.Worker.HostedService;

namespace StepEbay.Worker.Services
{
    public static class ServiceExtensions
    {

        //usually use AddTransient
        public static IServiceCollection AddService(this IServiceCollection services)
        {

            return services;
        }

        //usually use AddScoped
        public static IServiceCollection AddDbService(this IServiceCollection services)
        {

            return services;
        }

        //usually use AddTransient
        public static IServiceCollection AddRestService(this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddHubClients(this IServiceCollection services)
        {
            services.AddSingleton<BetHubClient>();

            return services;
        }

        public static IServiceCollection AddHosdedServices(this IServiceCollection services)
        {
            services.AddHostedService<BetService>();
            //services.AddHostedService<ImageCleanerService>();

            return services;
        }
    }
}
