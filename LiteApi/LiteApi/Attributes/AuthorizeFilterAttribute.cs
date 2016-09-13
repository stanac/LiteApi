using LiteApi.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LiteApi.Attributes
{
    /// <summary>
    /// Attribute that is used to check if user have access to the resource, can be set on controller or action. Also check <see cref="SkipAuthorizationAttribute"/>
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="LiteApi.Contracts.Abstractions.IApiFilter" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeFilterAttribute : Attribute, IApiFilter
    {
        private readonly Func<HttpContext, Task<bool>> _authorizeUserAsync;
        private readonly Func<HttpContext, bool> _authorizeUser;
        private readonly Claim[] _userMustHaveClaims;
        private readonly string[] _userMustHaveRoles;

        /// <summary>
        /// Gets or sets the response code that should be set in case API call should be prevented (controller/action should not be invoked).
        /// </summary>
        /// <value>
        /// The response code that should be set in case API call should be prevented (controller/action should not be invoked).
        /// </value>
        public int? ResponseCodeAfterRunWithoutContinuing { get; set; }

        /// <summary>
        /// Gets or sets whether response code should be set after run.
        /// </summary>
        /// <value>
        /// Whether response code should be set after run.
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
        /// <param name="userMustHaveClaims">The user must have claims.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AuthorizeFilterAttribute(params Claim[] userMustHaveClaims)
        {
            if (userMustHaveClaims == null) throw new ArgumentNullException(nameof(userMustHaveClaims));
            _userMustHaveClaims = userMustHaveClaims;
            _authorizeUser = UserHasClaims;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeFilterAttribute"/> class.
        /// </summary>
        /// <param name="userMustHaveRoles">The user must have roles.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AuthorizeFilterAttribute(params string[] userMustHaveRoles)
        {
            if (userMustHaveRoles == null) throw new ArgumentNullException(nameof(userMustHaveRoles));
            _userMustHaveRoles = userMustHaveRoles;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeFilterAttribute"/> class.
        /// </summary>
        /// <param name="authorizeUser">Custom function that checks if user is authorized to access the resource.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AuthorizeFilterAttribute(Func<HttpContext, bool> authorizeUser)
        {
            if (authorizeUser == null) throw new ArgumentNullException(nameof(authorizeUser));
            _authorizeUser = authorizeUser;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeFilterAttribute"/> class.
        /// </summary>
        /// <param name="authorizeUser">Custom function that checks if user is authorized to access the resource.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public AuthorizeFilterAttribute(Func<HttpContext, Task<bool>> authorizeUser)
        {
            if (authorizeUser == null) throw new ArgumentNullException(nameof(authorizeUser));
            _authorizeUserAsync = authorizeUser;
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
            if (_authorizeUserAsync == null)
            {
                result = _authorizeUser(httpCtx);
            }
            result = await _authorizeUserAsync(httpCtx);
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
                ResponseCodeAfterRunWithoutContinuing = 401;
            }
            return result;
        }

        private bool UserHasClaims(HttpContext httpCtx)
        {
            var claims = httpCtx?.User?.Claims?.ToArray();
            if (claims == null) claims = new Claim[0];
            foreach (var c in _userMustHaveClaims)
            {
                if (!claims.Any(x => x.Subject == c.Subject && x.Type == c.Type && x.Value == c.Value))
                {
                    ResponseCodeAfterRunWithoutContinuing = 403;
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
                ResponseCodeAfterRunWithoutContinuing = 403;
            }
            return result;
        }
        
    }
}
