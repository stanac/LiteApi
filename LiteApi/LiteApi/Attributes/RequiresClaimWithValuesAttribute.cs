using System;
using System.Linq;
using LiteApi.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Authorization filter where claim has to have all the provided values
    /// </summary>
    /// <seealso cref="LiteApi.Attributes.RequiresAuthenticationAttribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RequiresClaimWithValuesAttribute : RequiresAuthenticationAttribute
    {
        /// <summary>
        /// Claim type from constructor
        /// </summary>
        protected readonly string _claimType;

        /// <summary>
        /// Claim values from constructor
        /// </summary>
        protected readonly string[] _claimValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="RequiresClaimWithValuesAttribute"/> class.
        /// </summary>
        /// <param name="claimType">Type of the claim.</param>
        /// <param name="claimValues">The claim values to check.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException">
        /// </exception>
        public RequiresClaimWithValuesAttribute(string claimType, params string[] claimValues)
        {
            if (claimValues == null) throw new ArgumentNullException(nameof(claimValues));

            if (string.IsNullOrWhiteSpace(claimType)) throw new ArgumentException($"{nameof(claimType)} cannot be null or empty or white space");

            if (claimValues.Any(string.IsNullOrWhiteSpace) || claimValues.Length == 0)
            {
                throw new ArgumentException($"{nameof(claimValues)} cannot be empty or contain null or empty or white space value");
            }

            _claimType = claimType;
            _claimValues = claimValues;
        }

        /// <summary>
        /// Check if controller/action should be invoked or not. User must be and have claims with values in order for filter to pass.
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

            var claim = httpCtx.User.Claims.Where(x => x.Type == _claimType).ToArray();
            
            if (!CheckClaims(claim))
            {
                return ApiFilterRunResult.Unauthorized;
            }
            return result;
        }

        /// <summary>
        /// Checks the claims.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <returns>true if should continue</returns>
        protected virtual bool CheckClaims(Claim[] claims)
        {
            if (claims.Length == 0)
            {
                return false;
            }

            foreach (var cv in _claimValues)
            {
                if (!claims.Any(c => c.Value == cv))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
