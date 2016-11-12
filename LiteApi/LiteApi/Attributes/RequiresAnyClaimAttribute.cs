using LiteApi.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Checks if user has specified roles and instructs the middleware for execute action or not
    /// </summary>
    /// <seealso cref="LiteApi.Attributes.RequiresAuthenticationAttribute" />
    /// <seealso cref="LiteApi.Contracts.Abstractions.IApiFilter" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequiresAnyClaimAttribute : RequiresAuthenticationAttribute
    {
        private readonly string[] _claims;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresAnyClaimAttribute"/> class.
        /// </summary>
        /// <param name="claims">The claims to check</param>
        public RequiresAnyClaimAttribute(params string[] claims)
        {
            if (claims == null) throw new ArgumentNullException(nameof(claims));
            if (claims.Any(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException("Claims cannot be null or empty or white space.");
            }

            _claims = claims;
        }


        /// <summary>
        /// Check if controller/action should be invoked or not. User must be authenticated and have at least one of the specific claims in order for filter to pass.
        /// </summary>
        /// <param name="httpCtx">HTTP context</param>
        /// <returns>
        /// Should continue and if not which status code and message to set.
        /// </returns>
        public override ApiFilterRunResult ShouldContinue(HttpContext httpCtx)
        {
            var result = base.ShouldContinue(httpCtx);
            if (!result.ShouldContinue)
            {
                return result;
            }

            bool hasClaims = _claims.Any(x => httpCtx.User.Claims.Any(y => y.Type == x));
            if (!hasClaims)
            {
                result = ApiFilterRunResult.Unauthorized;
            }
            return result;
        }
    }
}
