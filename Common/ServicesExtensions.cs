using Microsoft.Extensions.DependencyInjection;
using StepEbay.Common.Lockers;
using StepEbay.Common.Storages;


namespace StepEbay.Common
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddStorages(this IServiceCollection services)
        {
            services.AddScoped<StorageQueue>();
            services.AddScoped(p => new CookieLocker(1));
            services.AddScoped<LocalStorage>();
            services.AddTransient<CookieStorage>();

            return services;
        }
    }
}
