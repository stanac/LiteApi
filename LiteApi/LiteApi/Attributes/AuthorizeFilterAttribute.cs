using LiteApi.Contracts.Abstractions;
using LiteApi.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Reflection;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Attribute that is used to check if user have access to the resource, can be set on controller or action. Also check <see cref="SkipAuthenticationAttribute"/>
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="IApiFilter" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeFilterAttribute : Attribute, IApiFilter
    {
        private readonly Func<HttpContext, Task<bool>> _authorizeUserAsync;
        private readonly Func<HttpContext, bool> _authorizeUser;
        private readonly Claim[] _userMustHaveClaims;
        private readonly string[] _userMustHaveRoles;
        private static readonly ObjectBuilder _objectBuilder = new ObjectBuilder();

        /// <summary>
        /// Gets or sets the response code that should be set in case API call should be prevented (controller/action should not be invoked).
        /// </summary>
        /// <value>
        /// The response code that should be set in case API call should be prevented (controller/action should not be invoked).
        /// </value>
        public int? SetHttpResponseCodeAfterRun { get; set; }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeFilterAttribute"/> class.
        /// </summary>
        public AuthorizeFilterAttribute()
        {
            _authorizeUserAsync = IsUserLoggedIn;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeFilterAttribute"/> class.
        /// </summary>
        /// <param name="userMustHaveRoles">Roles that user must have in order to be authorized.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AuthorizeFilterAttribute(params string[] userMustHaveRoles)
        {
            if (userMustHaveRoles == null) throw new ArgumentNullException(nameof(userMustHaveRoles));
            _userMustHaveRoles = userMustHaveRoles;
            _authorizeUser = UserHasRoles;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeFilterAttribute"/> class with claims validation.
        /// </summary>
        /// <param name="claimSeparator">The claim separator used in parameter userMustHaveClaims.</param>
        /// <param name="userMustHaveClaims">Claims that user must have in order to be authorized.</param>
        /// <example>
        /// [AuthorizeFilter(':', "myClaimType1:myClaimValue1", "myClaimType1:myClaimValue1")]
        /// </example>
        public AuthorizeFilterAttribute(char claimSeparator, params string[] userMustHaveClaims)
        {
            _userMustHaveClaims = userMustHaveClaims.Select(x =>
            {
                var values = x.Split(claimSeparator);
                if (values.Length != 2) throw new ArgumentException(
                    $"Value for claim {x ?? "-null-"} is not valid with claimSeparator {claimSeparator}. Attribute should be "
                    + "initialized as e.g. [AuthorizeFilter(':', \"myClaimType1:myClaimValue1\", \"myClaimType1:myClaimValue1\")]");
                return new Claim(values[0], values[1]);
            }).ToArray();
            _authorizeUser = UserHasClaims;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeFilterAttribute"/> class.
        /// </summary>
        /// <param name="customFilterImplementationType">The custom filter implementation, type must implement <see cref="ICustomApiFilter"/>.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException"></exception>
        public AuthorizeFilterAttribute(Type customFilterImplementationType)
        {
            if (customFilterImplementationType == null) throw new ArgumentNullException(nameof(customFilterImplementationType));
            if (!typeof(ICustomApiFilter).IsAssignableFrom(customFilterImplementationType))
            {
                throw new ArgumentException($"Provided type {customFilterImplementationType} is not implementing ICustomApiFilter");
            }
            var filter = _objectBuilder.BuildObject(customFilterImplementationType) as ICustomApiFilter;
            if (filter.IsAsync)
            {
                if (filter.ShouldContinueAsync == null) throw new ArgumentException($"Filter {customFilterImplementationType} is marked as async and ShouldContinueAsync is not implemented.");
                _authorizeUserAsync = filter.ShouldContinueAsync;
            }
            else
            {
                if (filter.ShouldContinue == null) throw new ArgumentException($"Filter {customFilterImplementationType} is marked as not async and ShouldContinue is not implemented.");
                _authorizeUser = filter.ShouldContinue;
            }
        }

        /// <summary>
        /// Checks if controller/action should be invoked or not (e.g. for authorization/authentication)
        /// </summary>
        /// <param name="httpCtx">The HTTP context provided by the middleware.</param>
        /// <returns>
        /// Pair of values, should continue and if not which status code to set.
        /// </returns>
        public async Task<ApiFilterRunResult> ShouldContinueAsync(HttpContext httpCtx)
        {
            bool result;
            if (_authorizeUserAsync != null)
            {
                result = await _authorizeUserAsync(httpCtx);
            }
            else
            {
                result = _authorizeUser(httpCtx);
            }
            return new ApiFilterRunResult
            {
                SetResponseCode = SetHttpResponseCodeAfterRun,
                ShouldContinue = result
            };
        }

        private async Task<bool> IsUserLoggedIn(HttpContext httpCtx)
        {
            bool result = await Task.Run(() => httpCtx?.User?.Identities?.FirstOrDefault().IsAuthenticated ?? false);
            if (!result)
            {
                SetHttpResponseCodeAfterRun = 401;
            }
            return result;
        }

        private bool UserHasClaims(HttpContext httpCtx)
        {
            var claims = httpCtx?.User?.Claims?.ToArray();
            if (claims == null) claims = new Claim[0];
            foreach (var c in _userMustHaveClaims)
            {
                if (!claims.Any(x => x.Type == c.Type && x.Value == c.Value))
                {
                    SetHttpResponseCodeAfterRun = 403;
                    return false;
                }
            }
            return true;
        }

        private bool UserHasRoles(HttpContext httpCtx)
        {
            bool result = _userMustHaveRoles.All(x => httpCtx?.User?.IsInRole(x) ?? false);
            if (!result)
            {
                SetHttpResponseCodeAfterRun = 403;
            }
            return result;
        }
        
    }
}
