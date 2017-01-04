using LiteApi.Contracts.Abstractions;
using System;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Validates access to controller/action. User must be authenticated in order for filter to pass.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="LiteApi.Contracts.Abstractions.IPolicyApiFilter" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequiresAuthorizationPolicyAttribute : Attribute, IPolicyApiFilter
    {
        /// <summary>
        /// Gets the name of the policy.
        /// </summary>
        /// <value>
        /// The name of the policy.
        /// </value>
        public string PolicyName { get; private set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresAuthorizationPolicyAttribute"/> class.
        /// </summary>
        /// <param name="policyName">Name of the policy.</param>
        public RequiresAuthorizationPolicyAttribute(string policyName)
        {
            if (string.IsNullOrWhiteSpace(policyName)) throw new ArgumentException("policyName cannot be null or empty or whitespace");
            PolicyName = policyName;
        }

        /// <summary>
        /// Checks if user meets authorization policy
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="policyStoreFactory">The policy store.</param>
        /// <returns>
        /// If action should be invoked, and if not, which status code and message to set
        /// </returns>
        public ApiFilterRunResult ShouldContinue(ClaimsPrincipal user, Func<IAuthorizationPolicyStore> policyStoreFactory)
        {
            var isAuthenticated = user?.Identity?.IsAuthenticated ?? false;
            if (!isAuthenticated)
            {
                return ApiFilterRunResult.Unauthenticated;
            }
            var policy = policyStoreFactory().GetPolicy(PolicyName);
            if (policy == null)
            {
                throw new Exception($"Policy with name {PolicyName} not found");
            }
            return policy(user)
                ? ApiFilterRunResult.Continue
                : ApiFilterRunResult.Unauthorized;
        }
    }
}
