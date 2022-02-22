using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using StepEbay.Main.Client.Common.RestServices;
using StepEbay.Main.Client.Services;
using StepEbay.Main.Client.Common.Providers;
using StepEbay.Common.Storages;
using StepEbay.Common;
using StepEbay.Main.Client.Common.Options;
using Microsoft.AspNetCore.Components.Authorization;
using StepEbay.Common.Lockers;

namespace StepEbay.Main.Client
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddTransient<HttpClient>();
            services.AddScoped<IApiService, ApiService>();

            services.AddStorages();

            CookieOptions cookieOptions = Configuration.GetSection("CookieOptions").Get<CookieOptions>();
            DomainOptions domainOptions = Configuration.GetSection("DomainOptions").Get<DomainOptions>();

            services.AddAuthorization();
            services.AddScoped<ITokenProvider, TokenProvider>(p => new TokenProvider(p.GetService<CookieStorage>(),
               cookieOptions.AccessToken, cookieOptions.RefreshToken, cookieOptions.Expires, domainOptions.Cookie));

            services.AddScoped<AuthenticationStateProvider>(p => (TokenProvider)p.GetService<ITokenProvider>());
            services.AddScoped<SemaphoreManager>();

            services.AddRazorPages();
            services.AddServerSideBlazor();

            services.Configure<DomainOptions>(Configuration.GetSection("DomainOptions"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
