using LiteApi.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LiteApi.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeFilterAttribute : Attribute, IApiFilter
    {
        private readonly Func<HttpContext, Task<bool>> _authorizeUserAsync;
        private readonly Func<HttpContext, bool> _authorizeUser;
        private readonly Claim[] _userMustHaveClaims;
        private readonly string[] _userMustHaveRoles;

        public int? ResponseCodeAfterRunWithoutContinuing { get; set; }
        
        public int? SetHttpResponseCodeAfterRun { get; set; }

        public AuthorizeFilterAttribute()
        {
            _authorizeUserAsync = IsUserLoggedIn;
        }

        public AuthorizeFilterAttribute(params Claim[] userMustHaveClaims)
        {
            if (userMustHaveClaims == null) throw new ArgumentNullException(nameof(userMustHaveClaims));
            _userMustHaveClaims = userMustHaveClaims;
            _authorizeUser = UserHasClaims;
        }

        public AuthorizeFilterAttribute(params string[] userMustHaveRoles)
        {
            if (userMustHaveRoles == null) throw new ArgumentNullException(nameof(userMustHaveRoles));
            _userMustHaveRoles = userMustHaveRoles;
        }

        public AuthorizeFilterAttribute(Func<HttpContext, bool> authorizeUser)
        {
            if (authorizeUser == null) throw new ArgumentNullException(nameof(authorizeUser));
            _authorizeUser = authorizeUser;
        }

        public AuthorizeFilterAttribute(Func<HttpContext, Task<bool>> authorizeUser)
        {
            if (authorizeUser == null) throw new ArgumentNullException(nameof(authorizeUser));
            _authorizeUserAsync = authorizeUser;
        }

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
