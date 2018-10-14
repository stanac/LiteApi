using LiteApi.Contracts.Abstractions;
using LiteApi.Tests.ModelSetup;
using System;
using System.Security.Claims;
using Xunit;

namespace LiteApi.Tests.FilterTests
{
    public class RequiresClaimWithAnyValueTests
    {
        [Fact]
        public void RequiresClaimWithAnyValue_UnauthenticatedUser_ReturnsUnauthenticated()
        {
            var attr = new RequiresClaimWithAnyValueAttribute("a", "1", "2", "3");
            var user = new ClaimsPrincipal();
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthenticated.SetResponseCode, shouldContinue.SetResponseCode);
        }
        
        [Fact]
        public void RequiresClaimWithAnyValue_UserWihtoutClaims_ReturnsUnauthorized()
        {
            var attr = new RequiresClaimWithAnyValueAttribute("a", "1", "2", "3");
            var user = UserSetup.GetUser();
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthorized.SetResponseCode, shouldContinue.SetResponseCode);
        }

        [Fact]
        public void RequiresClaimWithAnyValue_UserWithClaimsWithWrongValue_ReturnsUnauthorized()
        {
            var attr = new RequiresClaimWithAnyValueAttribute("a", "1", "2", "3");
            var user = UserSetup.GetUserWithClaims("a:4");
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthorized.SetResponseCode, shouldContinue.SetResponseCode);
        }
        
        [Fact]
        public void RequiresClaimWithAnyValue_UserWihtClaimWithProperValue_ReturnsContinue()
        {
            var attr = new RequiresClaimWithAnyValueAttribute("a", "1", "2", "3");
            var user = UserSetup.GetUserWithClaims("a:8", "a:1");
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.True(shouldContinue.ShouldContinue);
        }


    }
}
