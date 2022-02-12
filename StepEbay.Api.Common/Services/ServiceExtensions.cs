using Microsoft.Extensions.DependencyInjection;
using StepEbay.Data.Common.Services.UserDbServices;
using StepEbay.Main.Api.Common.Services.AuthServices;


namespace StepEbay.Main.Api.Common.Services
{
    public static class ServiceExtensions
    {

        //usually use AddTransient
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();

            return services;
        }

        //usually use AddScoped
        public static IServiceCollection AddDbService(this IServiceCollection services)
        {
            services.AddScoped<IUserDbService, UserDbService>();
            return services;
        }

        //usually use AddTransient
        public static IServiceCollection AddRestService(this IServiceCollection services)
        {
            return services;
        }
    }
}
