using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using System;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Tests.Controllers
{
    [RequiresAuthentication]
    public class SecureController : LiteController
    {
        public int Get1()
        {
            return 1;
        }

        [RequiresRoles("role1", "role2")]
        public int Get2()
        {
            return 2;
        }

        [RequiresClaims("claimType1", "claimType2")]
        public int Get3()
        {
            return 3;
        }

        [SkipFilters]
        public int Get4()
        {
            return 4;
        }

        [RequiresAuthorizationPolicy("Over16")]
        public int Get5()
        {
            return 5;
        }

        [RequiresAuthorizationPolicy("Over18")]
        public int Get6()
        {
            return 6;
        }

        [RequiresClaimWithValues("claimType1", "true")]
        [RequiresClaimWithValues("claimType2", "true")]
        public int Get7()
        {
            return 7;
        }

        [UserHasAnyTwoClaimsFilter]
        public int Get8()
        {
            return 8;
        }

        [SkipFilters]
        public int Get9()
        {
            return 9;
        }

        [UserHasAnyTwoClaimsAsyncFilter]
        public int Get10()
        {
            return 10;
        }

        [RequiresAnyRole("role1", "role2")]
        public int Get11()
        {
            return 11;
        }

        [RequiresAnyClaim("claim1", "claim2")]
        public int Get12()
        {
            return 12;
        }

        [RequiresClaimWithAnyValue("customClaim1", "value1", "value2")]
        public int Get13()
        {
            return 13;
        }
        
        [AttributeUsage(AttributeTargets.Method)]
        private class UserHasAnyTwoClaimsFilterAttribute : Attribute, IApiFilter
        {
            public bool IgnoreSkipFilters { get; set; } = false;

            public ApiFilterRunResult ShouldContinue(HttpContext httpCtx)
            {
                var userIsAuthenticated = httpCtx?.User?.Identity.IsAuthenticated ?? false;
                if (!userIsAuthenticated) return ApiFilterRunResult.Unauthenticated;

                return httpCtx.User.Claims.Count() > 1
                    ? ApiFilterRunResult.Continue
                    : ApiFilterRunResult.Unauthorized;
            }
        }

        [AttributeUsage(AttributeTargets.Method)]
        private class UserHasAnyTwoClaimsAsyncFilterAttribute : Attribute, IApiFilterAsync
        {
            public bool IgnoreSkipFilters { get; set; } = false;

            public Task<ApiFilterRunResult> ShouldContinueAsync(HttpContext httpCtx)
            {
                ApiFilterRunResult result = ApiFilterRunResult.Continue;
                var userIsAuthenticated = httpCtx?.User?.Identity.IsAuthenticated ?? false;
                if (!userIsAuthenticated) result = ApiFilterRunResult.Unauthenticated;
                else result = httpCtx.User.Claims.Count() > 1
                    ? ApiFilterRunResult.Continue
                    : ApiFilterRunResult.Unauthorized;
                return Task.FromResult(result);
            }
        }
    }

}
