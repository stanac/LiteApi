using LiteApi;
using System;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension for supporting fluent usage of the middleware
    /// </summary>
    public static class ApplicationBuilderExtenstions
    {
        /// <summary>
        /// Registers LiteApi middleware.
        /// </summary>
        /// <param name="appBuilder">Instance of <see cref="IApplicationBuilder"/></param>
        /// <returns>Instance of <see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseLiteApi(this IApplicationBuilder appBuilder)
            => UseLiteApi(appBuilder, LiteApiOptions.Default);

        /// <summary>
        /// Registers LiteApi middleware.
        /// </summary>
        /// <param name="appBuilder">Instance of <see cref="IApplicationBuilder"/></param>
        /// <param name="options">Instance of <see cref="LiteApiOptions"/> to use</param>
        /// <returns>Instance of <see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseLiteApi(this IApplicationBuilder appBuilder, LiteApiOptions options)
        {
            appBuilder.UseMiddleware<LiteApiMiddleware>(options, appBuilder.ApplicationServices);
            return appBuilder;
        }

        /// <summary>
        /// Registers LiteApi middleware
        /// </summary>
        /// <param name="appBuilder">Instance of <see cref="IApplicationBuilder"/></param>
        /// <param name="setOptions">Action to invoke to set <see cref="LiteApiOptions"/></param>
        /// <returns>Instance of <see cref="IApplicationBuilder"/></returns>
        public static IApplicationBuilder UseLiteApi(this IApplicationBuilder appBuilder, Action<LiteApiOptions> setOptions)
        {
            var options = LiteApiOptions.Default;
            setOptions(options);
            appBuilder.UseMiddleware<LiteApiMiddleware>(options, appBuilder.ApplicationServices);
            return appBuilder;
        }
    }
}
