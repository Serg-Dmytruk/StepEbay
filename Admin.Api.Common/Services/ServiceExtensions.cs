using Microsoft.Extensions.DependencyInjection;
using StepEbay.Admin.Api.Common.Services.DbSeeder;
using StepEbay.Admin.Api.Services.Telegram;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Common.Services.TelegramDbServices;

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
            services.AddScoped<IProductDbService, ProductDbService>();
            services.AddScoped<IProductStateDbService, ProductStateDbService>();    
            services.AddScoped<ICategoryDbService, CategoryDbService>();
            services.AddScoped<ISeeder, Seeder>();

            return services;
        }

        //usually use AddTransient
        public static IServiceCollection AddRestService(this IServiceCollection services)
        {
            return services;
        }
    }
}
