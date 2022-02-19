﻿using Microsoft.Extensions.DependencyInjection;
using StepEbay.Data.Common.Services.AuthDbServices;
using StepEbay.Data.Common.Services.UserDbServices;
using StepEbay.Main.Api.Common.Services.AuthServices;
using StepEbay.Main.Api.Common.Services.EmailSenderServices;

namespace StepEbay.Main.Api.Common.Services
{
    public static class ServiceExtensions
    {

        //usually use AddTransient
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();

            return services;
        }

        //usually use AddScoped
        public static IServiceCollection AddDbService(this IServiceCollection services)
        {
            services.AddScoped<IUserDbService, UserDbService>();
            services.AddScoped<IRefreshTokenDbService, RefreshTokenDbService>();
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
