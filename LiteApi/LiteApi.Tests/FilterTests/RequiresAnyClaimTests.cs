using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Tests.ModelSetup;
using System;
using System.Security.Claims;
using Xunit;

namespace LiteApi.Tests.FilterTests
{
    public class RequiresAnyClaimTests
    {
        [Fact]
        public void RequiresAnyClaim_NullClaims_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresAnyClaimAttribute(null);
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresAnyClaim_EmptyClaimsArray_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresAnyClaimAttribute();
            }
            catch (ArgumentException ex)
            {
                error = ex.Message.Contains("Claims array is empty");
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresAnyClaim_ClaimsContainingNullClaim_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresAnyClaimAttribute("a", null);
            }
            catch (ArgumentException ex)
            {
                error = ex.Message.Contains("Claims cannot be null or empty or white space");
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresAnyClaim_ClaimsContainingEmptyClaim_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresAnyClaimAttribute("a", "");
            }
            catch (ArgumentException ex)
            {
                error = ex.Message.Contains("Claims cannot be null or empty or white space");
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresAnyClaim_ClaimsContainingWhiteSpaceClaim_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresAnyClaimAttribute("a", "   ");
            }
            catch (ArgumentException ex)
            {
                error = ex.Message.Contains("Claims cannot be null or empty or white space");
            }
            Assert.True(error);
        }
        
        [Fact]
        public void RequiresAnyClaims_UnauthenticatedUser_ReturnsUnauthenticated()
        {
            var attr = new RequiresAnyClaimAttribute("a");
            var user = new ClaimsPrincipal();
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthenticated.SetResponseCode, shouldContinue.SetResponseCode);
        }

        [Fact]
        public void RequiresAnyClaims_UnauthorizedUser_ReturnsUnauthorized()
        {
            var attr = new RequiresAnyClaimAttribute("a");
            var user = UserSetup.GetUser();
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthorized.SetResponseCode, shouldContinue.SetResponseCode);
        }
        
        [Fact]
        public void RequiresAnyClaims_AuthorizedUserWithOneClaim_ReturnsAuthorized()
        {
            var attr = new RequiresAnyClaimAttribute("b", "c", "a");
            var user = UserSetup.GetUserWithClaims("a:0");
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.True(shouldContinue.ShouldContinue);
        }

        [Fact]
        public void RequiresAnyClaims_AuthorizedUserWithAllClaims_ReturnsAuthorized()
        {
            var attr = new RequiresAnyClaimAttribute("b", "c", "a");
            var user = UserSetup.GetUserWithClaims("a:0", "b:1", "c:2");
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.True(shouldContinue.ShouldContinue);
        }
    }
}
