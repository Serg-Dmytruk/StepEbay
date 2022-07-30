using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using StepEbay.Data.Common.Services.AuthDbServices;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Common.Services.UserDbServices;
using StepEbay.Main.Api.Common.Services.AuthServices;
using StepEbay.Main.Api.Common.Services.DataValidationServices;
using StepEbay.Main.Api.Common.Services.EmailSenderServices;
using StepEbay.Main.Api.Common.Services.ProductServices;
using StepEbay.Main.Common.Models.Auth;

namespace StepEbay.Main.Api.Common.Services
{
    public static class ServiceExtensions
    {

        //usually use AddTransient
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IValidator<SignUpRequestDto>, AuthValidator>();
            services.AddTransient<IProductService, ProductService>();
            return services;
        }

        //usually use AddScoped
        public static IServiceCollection AddDbService(this IServiceCollection services)
        {
            services.AddScoped<IUserDbService, UserDbService>();
            services.AddScoped<IProductDbService, ProductDbService>();
            services.AddScoped<ICategoryDbService, CategoryDbService>();
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
