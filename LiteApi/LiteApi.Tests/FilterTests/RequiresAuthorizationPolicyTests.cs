using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Tests.ModelSetup;
using System;
using System.Security.Claims;
using Xunit;

namespace LiteApi.Tests.FilterTests
{
    public class RequiresAuthorizationPolicyTests
    {
        [Fact]
        public void RequiersAuthorizationPolicy_NullPolicyName_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresAuthorizationPolicyAttribute(null);
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiersAuthorizationPolicy_EmptyPolicyName_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresAuthorizationPolicyAttribute("");
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiersAuthorizationPolicy_WhiteSpacePolicyName_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresAuthorizationPolicyAttribute(" ");
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresAuthorizationPolicy_NonExistingPolicy_ThrowsException()
        {
            var attr = new RequiresAuthorizationPolicyAttribute("policy");
            var user = UserSetup.GetUser();
            var policyStore = UserSetup.GetPolicyStore();
            bool error = false;
            try
            {
                attr.ShouldContinue(user, () => policyStore);
            }
            catch (Exception ex)
            {
                error = ex.Message.Contains("not found");
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresAuthorizationPolicy_NotLoggedInUser_ReturnsUnauthenticated()
        {
            var attr = new RequiresAuthorizationPolicyAttribute("policy");
            var user = new ClaimsPrincipal();
            var policyStore = UserSetup.GetPolicyStore();
            var shouldContinue = attr.ShouldContinue(user, () => policyStore);
            Assert.Equal(ApiFilterRunResult.Unauthenticated.SetResponseCode, shouldContinue.SetResponseCode);
        }

        [Fact]
        public void RequiresAuthorizationPolicy_UserWithoutPolicy_ReturnsUnauthorized()
        {
            var attr = new RequiresAuthorizationPolicyAttribute("policy");
            var user = UserSetup.GetUser();
            var policyStore = UserSetup.GetPolicyStore();
            policyStore.SetPolicy("policy", u => false);
            var shouldContinue = attr.ShouldContinue(user, () => policyStore);
            Assert.Equal(ApiFilterRunResult.Unauthorized.SetResponseCode, shouldContinue.SetResponseCode);
        }
        
        [Fact]
        public void RequiresAuthorizationPolicy_UserWithPolicy_ReturnsContinue()
        {
            var attr = new RequiresAuthorizationPolicyAttribute("policy");
            var user = UserSetup.GetUser();
            var policyStore = UserSetup.GetPolicyStore();
            policyStore.SetPolicy("policy", u => true);
            var shouldContinue = attr.ShouldContinue(user, () => policyStore);
            Assert.True(shouldContinue.ShouldContinue);
        }
    }
}
