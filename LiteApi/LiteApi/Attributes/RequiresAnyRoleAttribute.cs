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
    public class RequiresAnyRoleAttribute : RequiresAuthenticationAttribute
    {
        private readonly string[] _roles;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresRolesAttribute"/> class.
        /// </summary>
        /// <param name="roles">The roles to check</param>
        public RequiresAnyRoleAttribute(params string[] roles)
        {
            if (roles == null) throw new ArgumentNullException(nameof(roles));
            if (roles.Any(string.IsNullOrWhiteSpace) || roles.Length == 0)
            {
                throw new ArgumentException("Role cannot be null or empty or white space.");
            }

            _roles = roles.ToArray();
        }

        /// <summary>
        /// Check if controller/action should be invoked or not. User must be authenticated and have at least one of the specific roles in order for filter to pass.
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

            bool hasRoles = _roles.Any(httpCtx.User.IsInRole);
            if (!hasRoles)
            {
                result = ApiFilterRunResult.Unauthorized;
            }
            return result;
        }
    }
}
