using Microsoft.Extensions.DependencyInjection;
using StepEbay.Admin.Api.Common.Services.DbSeeder;
using StepEbay.Admin.Api.Services.Telegram;
using StepEbay.Data.Common.Services.ProductDbServices;
using StepEbay.Data.Common.Services.TelegramDbServices;
using StepEbay.Admin.Api.Common.Services.AuthServices;
using StepEbay.Data.Common.Services.UserDbServices;
using StepEbay.Data.Common.Services.AuthDbServices;
using StepEbay.Data.Common.Services.BetsDbServices;
using StepEbay.Admin.Api.Common.Services.Products;
using StepEbay.Admin.Api.Common.Services.UserServices;

namespace StepEbay.Admin.Api.Common.Services
{
    public static class ServiceExtensions
    {
        //usually use AddTransient
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddTransient<ITelegramService, TelegramService>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IUserService, UserService>();

            return services;
        }

        //usually use AddScoped
        public static IServiceCollection AddDbService(this IServiceCollection services)
        {
            services.AddScoped<IDeveloperGroupDbService, DeveloperGroupDbService>();
            services.AddScoped<IProductDbService, ProductDbService>();
            services.AddScoped<IProductStateDbService, ProductStateDbService>();
            services.AddScoped<IProductDescDbService, ProductDescDbService>();
            services.AddScoped<ICategoryDbService, CategoryDbService>();
            services.AddScoped<IPurchaseTypeDbService, PurchaseTypeDbService>();
            services.AddScoped<ISeeder, Seeder>();

            services.AddScoped<IUserDbService, UserDbService>();
            services.AddScoped<IRefreshTokenDbService,RefreshTokenDbService>();
            services.AddScoped<IRoleDbService, RoleDbService>();
            services.AddScoped<IUserRoleDbService, UserRoleDbService>();
            services.AddScoped<IPurchesDbService, PurchesDbService>();
            services.AddScoped<IPurchesStateDbService, PurchesStateDbService>();

            return services;
        }

        //usually use AddTransient
        public static IServiceCollection AddRestService(this IServiceCollection services)
        {
            return services;
        }
    }
}
