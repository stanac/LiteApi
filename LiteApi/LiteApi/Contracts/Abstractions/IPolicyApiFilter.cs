using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Interface for policy filter
    /// </summary>
    public interface IPolicyApiFilter
    {
        /// <summary>
        /// Gets the name of the policy.
        /// </summary>
        /// <value>
        /// The name of the policy.
        /// </value>
        string PolicyName { get; }

        /// <summary>
        /// Checks if user meets authorization policy
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="policyStoreFactory">The policy store factory.</param>
        /// <returns>If action should be invoked, and if not, which status code and message to set</returns>
        ApiFilterRunResult ShouldContinue(ClaimsPrincipal user, Func<IAuthorizationPolicyStore> policyStoreFactory);
    }
}
