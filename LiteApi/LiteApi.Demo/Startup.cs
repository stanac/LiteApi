using LiteApi.Contracts.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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

            var options = LiteApiOptions.Default
                    .SetLoggerFactory(loggerFactory)
                    .AddAdditionalQueryModelBinder(new StackQueryBinder());
            options.InternalServiceResolver.Register<IControllerDiscoverer, CustomControllerDiscoverer>();
            app.UseLiteApi(options);
            
            app.Run(async context =>
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("404 - NOT FOUND");
            });
        }
    }
}
