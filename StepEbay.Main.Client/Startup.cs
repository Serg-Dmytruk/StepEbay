using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StepEbay.Common;
using StepEbay.Common.Helpers;
using StepEbay.Common.Lockers;
using StepEbay.Common.Storages;
using StepEbay.Main.Client.Base.Providers;
using StepEbay.Main.Client.Common.ClientsHub;
using StepEbay.Main.Client.Common.Options;
using StepEbay.Main.Client.Common.Providers;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Client.Services;
using StepEbay.PushMessage.Services;
using System.Net.Http;

namespace StepEbay.Main.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddHttpContextAccessor();
            services.AddTransient<HttpClient>();
            services.AddScoped<IApiService, ApiService>();

            services.AddStorages();

            services.Configure<DomainOptions>(Configuration.GetSection("DomainOptions"));
            services.Configure<AccountOptions>(Configuration.GetSection("AccountOptions"));

            CookieOptions cookieOptions = Configuration.GetSection("CookieOptions").Get<CookieOptions>();
            DomainOptions domainOptions = Configuration.GetSection("DomainOptions").Get<DomainOptions>();

            services.AddAuthorization();
            services.AddScoped<ITokenProvider, TokenProvider>(p => new TokenProvider(p.GetService<CookieStorage>(),
                cookieOptions.AccessToken, cookieOptions.RefreshToken, cookieOptions.Expires, domainOptions.Cookie));

            services.AddScoped<TimezoneHelper>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<AuthenticationStateProvider>(p => (TokenProvider)p.GetService<ITokenProvider>());
            services.AddScoped<SemaphoreManager>();
            services.AddScoped<HubClient>();
            services.AddScoped<PriceHubClient>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
