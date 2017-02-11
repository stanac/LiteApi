using LiteApi.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace LiteApi.Services
{
    /// <summary>
    /// Gets and sets authorization policy by name
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IAuthorizationPolicyStore" />
    internal class AuthorizationPolicyStore : IAuthorizationPolicyStore
    {
        private Dictionary<string, Func<ClaimsPrincipal, bool>> _store = new Dictionary<string, Func<ClaimsPrincipal, bool>>();

        /// <summary>
        /// Gets the policy.
        /// </summary>
        /// <param name="name">The name of the policy.</param>
        /// <returns>
        /// Authorization policy
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Func<ClaimsPrincipal, bool> GetPolicy(string name)
        {
            Func<ClaimsPrincipal, bool> policy = null;
            if (_store.TryGetValue(name, out policy))
            {
                return policy;
            }
            return null;
        }

        /// <summary>
        /// Sets the policy.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="policy">The policy.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SetPolicy(string name, Func<ClaimsPrincipal, bool> policy)
        {
            _store[name] = policy;
        }
    }
}
