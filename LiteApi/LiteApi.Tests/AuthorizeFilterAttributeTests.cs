using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using LiteApi.Services;
using LiteApi.Services.ModelBinders;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class AuthorizeFilterAttributeTests
    {
        [Fact]
        public async Task SecuredController_NoUser_CannotAccess()
        {
            await AssertSecureControllerAccess(null, "Get1", 401);
        }

        [Fact]
        public async Task SecuredController_WithUser_CanAccess()
        {
            await AssertSecureControllerAccess(GetUser(), "Get1", 200);
        }

        [Fact]
        public async Task SecureController_UserWithoutRoles_CannotAccess()
        {
            var user = GetUser();
            await AssertSecureControllerAccess(user, "Get2", 403);
        }

        [Fact]
        public async Task SecureController_UserWithRoles_CanAccess()
        {
            var user = GetUser("role1", "role2");
            await AssertSecureControllerAccess(user, "Get2", 200);
        }

        [Fact]
        public async Task SecureController_UserWithoutBothClaims_CannotAccess()
        {
            var user = GetUserWithClaims("claimType1:true");
            await AssertSecureControllerAccess(user, "Get3", 403);
        }

        [Fact]
        public async Task SecureController_UserWithBothClaims_CanAccess()
        {
            var user = GetUserWithClaims("claimType1:true", "claimType2:true");
            await AssertSecureControllerAccess(user, "Get7", 200);
        }

        [Fact]
        public async Task SecureController_NoUser_CanAccessActionWithSkipAuthAttr()
        {
            await AssertSecureControllerAccess(null, "Get4", 200);
        }

        [Fact]
        public async Task SecureController_NoUser_CannotAccessActionWithCustomPolicy()
        {
            await AssertSecureControllerAccess(null, "Get6", 401);
        }

        //[Fact]
        //public async Task SecureController_NoUser_CannotAccessActionWithCustomAsyncFilter()
        //{
        //    await AssertSecureControllerAccess(null, "Get5", 401);
        //}

        [Fact]
        public async Task SecureController_UserWithoutClaims_CannotAccessActionWithCustomPolicy()
        {
            await AssertSecureControllerAccess(GetUserWithClaims(), "Get5", 403, GetPolicyStore());
        }

        //[Fact]
        //public async Task SecureController_UserWithoutClaims_CannotAccessActionWithCustomAsyncFilter()
        //{
        //    await AssertSecureControllerAccess(GetUserWithClaims(), "Get5", 403);
        //}

        //[Fact]
        //public async Task SecureController_UserWithClaims_CannotAccessActionWithCustomFilter()
        //{
        //    await AssertSecureControllerAccess(GetUserWithClaims("claimType1:true", "claimType2:true"), "Get5", 200);
        //}

        //[Fact]
        //public async Task SecureController_UserWithClaims_CannotAccessActionWithCustomAsyncFilter()
        //{
        //    await AssertSecureControllerAccess(GetUserWithClaims("claimType1:true", "claimType2:true"), "Get5", 200);
        //}

        private async Task AssertSecureControllerAccess(ClaimsPrincipal user, string method, int expectedStatusCode, IAuthorizationPolicyStore policyStore = null)
        {
            var ctrl = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.SecureController)).GetControllers(null).Single();
            if (policyStore != null)
            {
                object[] methodCallProps = { policyStore };
                typeof(ControllerContext)
                    .GetTypeInfo()
                    .GetProperty("AuthPolicyStore", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetMethod.Invoke(ctrl, methodCallProps);

            }
            var actionCtx = ctrl.Actions.Single(x => string.Compare(method, x.Name, StringComparison.OrdinalIgnoreCase) == 0);
            var invoker = new ActionInvoker(new ControllerBuilder(), new ModelBinderCollection(new JsonSerializer()));
            var httpCtx = new Fakes.FakeHttpContext();
            httpCtx.User = user;
            httpCtx.Request.Path = "/api/secure/" + method;
            await invoker.Invoke(httpCtx, actionCtx);
            Assert.Equal(expectedStatusCode, httpCtx.Response.StatusCode);
        }

        private ClaimsPrincipal GetUser(params string[] roles)
        {
            var claims = roles.Select(x => new Claim(ClaimTypes.Role, x));
            var user = new ClaimsPrincipal();
            user.AddIdentity(new ClaimsIdentity(claims, "test_auth"));
            
            return user;
        }

        private ClaimsPrincipal GetUserWithClaims(params string[] claims)
        {
            var claimValues = claims.Select(x =>
            {
                string[] values = x.Split(':');
                if (values.Length != 2) throw new Exception();
                return new Claim(values[0], values[1]);
            });
            var user = new ClaimsPrincipal();
            user.AddIdentity(new ClaimsIdentity(claimValues, "test_auth"));
            return user;
        }
        
        private IAuthorizationPolicyStore GetPolicyStore()
        {
            IAuthorizationPolicyStore store = new AuthorizationPolicyStore();
            store.SetPolicy("Over16", user =>
            {
                var claim = user.Claims.FirstOrDefault(x => x.Type == "age");
                if (claim != null)
                {
                    int val;
                    if (int.TryParse(claim.Value, out val))
                    {
                        return val >= 16;
                    }
                }
                return false;
            });
            store.SetPolicy("Over16", user =>
            {
                var claim = user.Claims.FirstOrDefault(x => x.Type == "age");
                if (claim != null)
                {
                    int val;
                    if (int.TryParse(claim.Value, out val))
                    {
                        return val >= 16;
                    }
                }
                return false;
            });
            return store;
        }
    }
    
}
