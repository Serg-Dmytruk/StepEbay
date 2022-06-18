using Microsoft.Extensions.DependencyInjection;
using StepEbay.Data.Common.Services.TelegramDbServices;
using StepEbay.Admin.Api.Services.Telegram;

namespace StepEbay.Admin.Api.Common.Services
{
    public static class ServiceExtensions
    {
        //usually use AddTransient
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddTransient<ITelegramService, TelegramService>();
            return services;
        }

        //usually use AddScoped
        public static IServiceCollection AddDbService(this IServiceCollection services)
        {
            services.AddScoped<IDeveloperGroupDbService, DeveloperGroupDbService>();
            return services;
        }

        //usually use AddTransient
        public static IServiceCollection AddRestService(this IServiceCollection services)
        {
            return services;
        }
    }
}
