using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Text.Encodings.Web;
using System.Linq;

namespace LiteApi.AuthSample
{
    public class Startup
    {
        public const string CookieAuthSchemeKey = "MyCookieMiddlewareInstance";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDataProtection();                // needed for cookie authentication to work
            services.AddScoped(_ => UrlEncoder.Default); // needed for cookie authentication to work
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationScheme = CookieAuthSchemeKey
            });

            app.UseStaticFiles();

            app.UseLiteApi(LiteApiOptions.Default
                .AddAuthorizationPolicy("AgeOver18", user =>
                {
                    // extension method, you would need to add "using LiteApi;" to use it.
                    var value = user.Claims.GetFirstNullableInt("Age");
                    return value.HasValue && value.Value >= 18;
                }));

            app.Use(next => async context =>
            {
                if (context.Request.Path == "/")
                {
                    context.Response.Redirect("/index.html");
                }
                else
                {
                    await next.Invoke(context);
                }
            });

            app.Run(async context =>
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("404 - NOT FOUND");
            });
        }
    }
}
