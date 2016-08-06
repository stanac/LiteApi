using Microsoft.AspNetCore.Builder;

namespace LiteApi
{
    public static class ApplicationBuilderExtenstions
    {
        public static void UseLiteApi(this IApplicationBuilder appBuilder)
            => UseLiteApi(appBuilder, LiteMiddlewareOptions.Default);

        public static void UseLiteApi(this IApplicationBuilder appBuilder, LiteMiddlewareOptions options)
        {
            appBuilder.UseMiddleware<LiteApiMiddleware>(options, appBuilder.ApplicationServices);
        }
    }
}
