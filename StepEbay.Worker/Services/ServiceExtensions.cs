using Microsoft.Extensions.DependencyInjection;

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
    }
}
