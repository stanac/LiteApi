using LiteApi.Contracts.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using LiteApi.Services;

namespace LiteApi.Demo
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDemoService, DemoService>();
            services.AddSingleton<IPersonDataAccess, PersonDataAccess>();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDebug();
            app.UseStaticFiles();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseLiteApi(options =>
            {
                options.SetLoggerFactory(loggerFactory);
                options.AddAdditionalQueryModelBinder(new StackQueryBinder(new LiteApiOptionsAccessor(options)));
                options.AddGlobalFilter(new TestGlobalFilter());
                options.InternalServiceResolver.Register<IControllerDiscoverer, CustomControllerDiscoverer>();
                options.SetDiscoveryEnabled(true);
            });
            
            app.Run(async context =>
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("404 - NOT FOUND, try going to /LiteApi/info");
            });
        }

        class TestGlobalFilter : IApiFilter
        {
            public bool IgnoreSkipFilters => false;

            public ApiFilterRunResult ShouldContinue(HttpContext httpCtx)
            {
                //return ApiFilterRunResult.Unauthenticated;
                return ApiFilterRunResult.Continue;
            }
        }
    }
}
