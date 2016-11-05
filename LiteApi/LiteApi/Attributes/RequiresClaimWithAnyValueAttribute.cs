using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Authorization filter where claim has to have any of the provided values
    /// </summary>
    /// <seealso cref="LiteApi.Attributes.RequiresAuthenticationAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequiresClaimWithAnyValueAttribute : RequiresClaimWithValuesAttribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresClaimWithAnyValueAttribute"/> class.
        /// </summary>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="claimValues">The claim values to check.</param>
        public RequiresClaimWithAnyValueAttribute(string claimType, params string[] claimValues) 
            : base(claimType, claimValues)
        {
        }

        /// <summary>
        /// Checks the claims.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <returns>
        /// true if should continue
        /// </returns>
        protected override bool CheckClaims(Claim[] claims)
        {
            return claims.Any(x => _claimValues.Any(y => y == x.Value));
        }
    }
}
