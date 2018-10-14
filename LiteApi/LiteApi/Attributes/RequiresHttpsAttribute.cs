using LiteApi.Contracts.Abstractions;
using System;
using Microsoft.AspNetCore.Http;

namespace LiteApi
{
    /// <summary>
    /// Requires HTTPS protocol to be used. By default this attribute ignores <see cref="SkipFiltersAttribute"/> unless
    /// property <see cref="IgnoreSkipFilters"/> is set to false.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="LiteApi.Contracts.Abstractions.IApiFilter" />
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class RequiresHttpsAttribute : Attribute, IApiFilter
    {
        /// <summary>
        /// Gets a value indicating whether <see cref="T:LiteApi.Attributes.SkipFiltersAttribute" /> should be ignored.
        /// </summary>
        /// <value>
        /// <c>true</c> if SkipFiltersAttribute should be ignored; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreSkipFilters { get; set; } = true;

        /// <summary>
        /// Can be called to check if controller/action should be invoked or not (e.g. for authorization/authentication)
        /// </summary>
        /// <param name="httpCtx">HTTP context</param>
        /// <returns>
        /// Should continue and if not which status code and message to set.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public ApiFilterRunResult ShouldContinue(HttpContext httpCtx)
        {
            if (httpCtx.Request.IsHttps) return ApiFilterRunResult.Continue;

            return new ApiFilterRunResult
            {
                SetResponseCode = 400,
                SetResponseMessage = "Bad request, HTTPS request was expected.",
                ShouldContinue = false
            };
        }
    }
}
