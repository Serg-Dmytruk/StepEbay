using Microsoft.Extensions.DependencyInjection;
using StepEbay.Data.Common.Services.TelegramDbServices;
using StepEbay.Admin.Api.Services.Telegram;
using StepEbay.Admin.Api.Common.Services.AuthServices;
using StepEbay.Data.Common.Services.UserDbServices;
using StepEbay.Data.Common.Services.AuthDbServices;

namespace StepEbay.Admin.Api.Common.Services
{
    public static class ServiceExtensions
    {
        //usually use AddTransient
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddTransient<ITelegramService, TelegramService>();
            services.AddTransient<IAuthService, AuthService>();

            return services;
        }

        //usually use AddScoped
        public static IServiceCollection AddDbService(this IServiceCollection services)
        {
            services.AddScoped<IDeveloperGroupDbService, DeveloperGroupDbService>();
            services.AddScoped<IUserDbService, UserDbService>();
            services.AddScoped<IRefreshTokenDbService,RefreshTokenDbService>();
            services.AddScoped<IRoleDbService, RoleDbService>();
             
            return services;
        }

        //usually use AddTransient
        public static IServiceCollection AddRestService(this IServiceCollection services)
        {
            return services;
        }
    }
}
