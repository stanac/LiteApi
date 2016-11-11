using System;
using System.Security.Claims;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Gets and sets authorization policy by name
    /// </summary>
    public interface IAuthorizationPolicyStore
    {
        /// <summary>
        /// Gets the policy.
        /// </summary>
        /// <param name="name">The name of the policy.</param>
        /// <returns>Authorization policy</returns>
        Func<ClaimsPrincipal, bool> GetPolicy(string name);

        /// <summary>
        /// Sets the policy.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="policy">The policy.</param>
        void SetPolicy(string name, Func<ClaimsPrincipal, bool> policy);
    }
}
