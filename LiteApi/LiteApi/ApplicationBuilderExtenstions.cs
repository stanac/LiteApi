using Microsoft.AspNetCore.Builder;

namespace LiteApi
{
    public static class ApplicationBuilderExtenstions
    {
        public static IApplicationBuilder UseLiteApi(this IApplicationBuilder appBuilder)
            => UseLiteApi(appBuilder, LiteApiOptions.Default);

        public static IApplicationBuilder UseLiteApi(this IApplicationBuilder appBuilder, LiteApiOptions options)
        {
            appBuilder.UseMiddleware<LiteApiMiddleware>(options, appBuilder.ApplicationServices);
            return appBuilder;
        }
    }
}
