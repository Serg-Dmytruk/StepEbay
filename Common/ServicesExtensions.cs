using Microsoft.Extensions.DependencyInjection;
using StepEbay.Common.Lockers;
using StepEbay.Common.Storages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepEbay.Common
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddStorages(this IServiceCollection services)
        {
            services.AddScoped<StorageQueue>();
            services.AddScoped(p => new CookieLocker(1));
            services.AddTransient<CookieStorage>();

            return services;
        }
    }
}
