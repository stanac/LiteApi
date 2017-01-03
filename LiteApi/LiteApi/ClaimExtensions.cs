using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace LiteApi
{
    /// <summary>
    /// Extensions that can be used for defining custom authorization policies.
    /// </summary>
    public static class ClaimExtensions
    {
        /// <summary>
        /// Gets the first found claim as nullable int.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="claimType">Type of the claim to search for.</param>
        /// <returns>value if found and parsed otherwise null</returns>
        public static int? GetFirstNullableInt(this IEnumerable<Claim> claims, string claimType)
        {
            var claim = claims.FirstOrDefault(x => x.Type == claimType);
            if (claim != null)
            {
                int val;
                if (int.TryParse(claim.Value, out val))
                {
                    return val;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the first found claim value as string.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <param name="claimType">Type of the claim to search for.</param>
        /// <returns>value if found otherwise null</returns>
        public static string GetFirstAsString(this IEnumerable<Claim> claims, string claimType)
        {
            var claim = claims.FirstOrDefault(x => x.Type == claimType);
            if (claim != null)
            {
                return claim.Value;
            }
            return null;
        }
    }
}
