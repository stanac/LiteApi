using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using LiteApi.Services;
using LiteApi.Services.ModelBinders;
using LiteApi.Tests.ModelSetup;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests.FilterTests
{
    public class AuthFlowTests
    {
        [Fact]
        public async Task SecuredController_NoUser_CannotAccess()
        {
            await AssertSecureControllerAccess(null, "Get1", 401);
        }

        [Fact]
        public async Task SecuredController_WithUser_CanAccess()
        {
            await AssertSecureControllerAccess(UserSetup.GetUser(), "Get1", 200);
        }

        [Fact]
        public async Task SecureController_UserWithoutRoles_CannotAccess()
        {
            var user = UserSetup.GetUser();
            await AssertSecureControllerAccess(user, "Get2", 403);
        }

        [Fact]
        public async Task SecureController_UserWithRoles_CanAccess()
        {
            var user = UserSetup.GetUser("role1", "role2");
            await AssertSecureControllerAccess(user, "Get2", 200);
        }

        [Fact]
        public async Task SecureController_UserWithOneRoles_CannotAccess()
        {
            var user = UserSetup.GetUser("role1");
            await AssertSecureControllerAccess(user, "Get2", 403);
        }

        [Fact]
        public async Task SecureController_UserWithoutBothClaims_CannotAccess()
        {
            var user = UserSetup.GetUserWithClaims("claimType1:true");
            await AssertSecureControllerAccess(user, "Get3", 403);
        }

        [Fact]
        public async Task SecureController_UserWithBothClaims_CanAccess()
        {
            var user = UserSetup.GetUserWithClaims("claimType1:true", "claimType2:true");
            await AssertSecureControllerAccess(user, "Get7", 200);
        }

        [Fact]
        public async Task SecureController_UserWithOneClaims_CannotAccess()
        {
            var user = UserSetup.GetUserWithClaims("claimType1:true");
            await AssertSecureControllerAccess(user, "Get7", 403);
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

        [Fact]
        public async Task SecureController_NoUser_CannotAccessActionWithCustomFilter()
        {
            await AssertSecureControllerAccess(null, "Get5", 401, UserSetup.GetPolicyStore());
        }

        [Fact]
        public async Task SecureController_UserWithoutClaims_CannotAccessActionWithCustomPolicy()
        {
            await AssertSecureControllerAccess(UserSetup.GetUserWithClaims(), "Get5", 403, UserSetup.GetPolicyStore());
        }

        [Fact]
        public async Task SecureController_UserWithValidClaim_CanAccessActionWithCustomPolicy()
        {
            await AssertSecureControllerAccess(UserSetup.GetUserWithClaims("age:18"), "Get5", 200, UserSetup.GetPolicyStore());
        }

        [Fact]
        public async Task SecureController_UserWithInvalidClaim_CanAccessActionWithCustomPolicy()
        {
            await AssertSecureControllerAccess(UserSetup.GetUserWithClaims("age:16"), "Get6", 403, UserSetup.GetPolicyStore());
        }

        [Fact]
        public async Task SecureController_NoUser_CannotAccessActionWithCustomAttribute()
        {
            await AssertSecureControllerAccess(null, "Get8", 401);
        }

        [Fact]
        public async Task SecureController_UserWithoutValidClaims_CannotAccessActionWithCustomAttribute()
        {
            await AssertSecureControllerAccess(UserSetup.GetUserWithClaims(), "Get8", 403);
        }

        [Fact]
        public async Task SecureController_UserWithValidClaims_CanAccessActionWithCustomAttribute()
        {
            await AssertSecureControllerAccess(UserSetup.GetUserWithClaims("a:1", "b:2"), "Get8", 200);
        }

        [Fact]
        public async Task SecureController_NoUser_CanAccessWithSkipFilterAttribute()
        {
            await AssertSecureControllerAccess(null, "Get9", 200);
        }
        
        [Fact]
        public async Task SecureController_User_CanAccessWithSkipFilterAttribute()
        {
            await AssertSecureControllerAccess(UserSetup.GetUserWithClaims("a:1", "b:2"), "Get9", 200);
        }

        [Fact]
        public async Task SecureController_NoUser_CannotAccessActionWithCustomAsyncAttribute()
        {
            await AssertSecureControllerAccess(null, "Get10", 401);
        }

        [Fact]
        public async Task SecureController_UserWithoutValidClaims_CannotAccessActionWithCustomAsyncAttribute()
        {
            await AssertSecureControllerAccess(UserSetup.GetUserWithClaims(), "Get10", 403);
        }

        [Fact]
        public async Task SecureController_UserWithValidClaims_CanAccessActionWithCustomAsyncAttribute()
        {
            await AssertSecureControllerAccess(UserSetup.GetUserWithClaims("a:1", "b:2"), "Get10", 200);
        }

        [Fact]
        public async Task SecureController_UserWithoutAnyRole_CannotAccessRequiresAnyRole()
        {
            await AssertSecureControllerAccess(UserSetup.GetUser(), "Get11", 403);
        }
        
        [Fact]
        public async Task SecureController_UserWithOneRole_CanAccessRequiresAnyRole()
        {
            await AssertSecureControllerAccess(UserSetup.GetUser("role2"), "Get11", 200);
        }

        [Fact]
        public async Task SecureController_UserWithTwoRoles_CanAccessRequiresAnyRole()
        {
            await AssertSecureControllerAccess(UserSetup.GetUser("role2", "role1"), "Get11", 200);
        }
        
        [Fact]
        public async Task SecureController_UserWithoutAnyClaim_CannotAccessRequiresAnyClaim()
        {
            await AssertSecureControllerAccess(UserSetup.GetUserWithClaims(), "Get12", 403);
        }

        [Fact]
        public async Task SecureController_UserWithOneClaim_CanAccessRequiresAnyClaim()
        {
            await AssertSecureControllerAccess(UserSetup.GetUserWithClaims("claim1:0"), "Get12", 200);
        }

        [Fact]
        public async Task SecureController_UserWithTwoClaims_CanAccessRequiresAnyClaim()
        {
            await AssertSecureControllerAccess(UserSetup.GetUserWithClaims("claim1:0", "claim2:0"), "Get12", 200);
        }
        
        [Fact]
        public async Task SecureController_UserWithoutAnyClaim_CannotAccessRequiresClaimWithAnyValue()
        {
            await AssertSecureControllerAccess(UserSetup.GetUserWithClaims(), "Get13", 403);
        }

        [Fact]
        public async Task SecureController_UserWithClaimWithOneValue_CanAccessRequiresClaimWithAnyValue()
        {
            await AssertSecureControllerAccess(UserSetup.GetUserWithClaims("customClaim1:value1"), "Get13", 200);
        }

        [Fact]
        public async Task SecureController_UserWithClaimWithTwoValues_CanAccessRequiresClaimWithAnyValue()
        {
            await AssertSecureControllerAccess(UserSetup.GetUserWithClaims("customClaim1:value1", "claim1:value2"), "Get13", 200);
        }

        private async Task AssertSecureControllerAccess(ClaimsPrincipal user, string method, int expectedStatusCode, IAuthorizationPolicyStore policyStore = null)
        {
            var ctrl = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.SecureController)).GetControllers(null).Single();
            if (policyStore != null)
            {
                var options = LiteApiOptions.Default;
                foreach (var policy in policyStore.GetPolicyNames())
                {
                    options.AuthorizationPolicyStore.SetPolicy(policy, policyStore.GetPolicy(policy));
                }
                ctrl.Filters = null; // force refresh init with new policy store
                foreach (var action in ctrl.Actions)
                {
                    action.Filters = null;
                }
                ctrl.Init(new LiteApiOptionsRetriever(options));
            }

            var actionCtx = ctrl.Actions.Single(x => string.Compare(method, x.Name, StringComparison.OrdinalIgnoreCase) == 0);
            var invoker = new ActionInvoker(new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object), new ModelBinderCollection(new JsonSerializer(), Fakes.FakeServiceProvider.GetServiceProvider()), new JsonSerializer());
            var httpCtx = new Fakes.FakeHttpContext();
            httpCtx.User = user;
            httpCtx.Request.Path = "/api/secure/" + method;
            await invoker.Invoke(httpCtx, actionCtx);
            Assert.Equal(expectedStatusCode, httpCtx.Response.StatusCode);
        }

    }
    
}
