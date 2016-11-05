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
        /// Check if controller/action should be invoked or not. User must be authenticated in order for filter to pass.
        /// </summary>
        /// <param name="httpCtx">HTTP context</param>
        /// <returns>
        /// Should continue and if not which status code and message to set.
        /// </returns>
        public virtual ApiFilterRunResult ShouldContinue(HttpContext httpCtx)
        {
            var isAuthenticated = httpCtx?.User?.Identity?.IsAuthenticated ?? false;
            var result = new ApiFilterRunResult
            {
                ShouldContinue = true
            };
            if (isAuthenticated)
            {
                result.ShouldContinue = false;
                result.SetResponseCode = 401;
                result.SetResponseMessage = "This resource requires authenticated user";
            }
            return result;
        }
    }
}
