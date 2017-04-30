using LiteApi.Contracts.Abstractions;
using System;
using Microsoft.AspNetCore.Http;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Validates access to controller/action. User must be authenticated in order for filter to pass.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="LiteApi.Contracts.Abstractions.IApiFilter" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequiresAuthenticationAttribute : Attribute, IApiFilter
    {
        /// <summary>
        /// Gets a value indicating whether <see cref="T:LiteApi.Attributes.SkipFiltersAttribute" /> should be ignored.
        /// </summary>
        /// <value>
        /// <c>true</c> if SkipFiltersAttribute should be ignored; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreSkipFilters => false;

        /// <summary>
        /// Check if controller/action should be invoked or not. User must be authenticated in order for filter to pass.
        /// </summary>
        /// <param name="httpCtx">HTTP context</param>
        /// <returns>
        /// Should continue and if not which status code and message to set.
        /// </returns>
        public virtual ApiFilterRunResult ShouldContinue(HttpContext httpCtx)
        {
            var isAuthenticated = httpCtx?.User?.Identity?.IsAuthenticated ?? false;
            return isAuthenticated
                ? ApiFilterRunResult.Continue
                : ApiFilterRunResult.Unauthenticated;
        }
    }
}
