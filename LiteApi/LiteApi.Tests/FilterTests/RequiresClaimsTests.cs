using LiteApi.Contracts.Abstractions;
using LiteApi.Tests.ModelSetup;
using System;
using System.Security.Claims;
using Xunit;

namespace LiteApi.Tests.FilterTests
{
    public class RequiresClaimsTests
    {
        [Fact]
        public void RequiresClaims_NullClaims_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresClaimsAttribute(null);
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresClaims_EmptyClaimsArray_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresClaimsAttribute();
            }
            catch (ArgumentException ex)
            {
                error = ex.Message.Contains("Claims array is empty");
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresClaims_ClaimsContainingNullClaim_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresClaimsAttribute("a", null);
            }
            catch (ArgumentException ex)
            {
                error = ex.Message.Contains("Claims cannot be null or empty or white space");
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresClaims_ClaimsContainingEmptyClaim_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresClaimsAttribute("a", "");
            }
            catch (ArgumentException ex)
            {
                error = ex.Message.Contains("Claims cannot be null or empty or white space");
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresClaims_ClaimsContainingWhiteSpaceClaim_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresClaimsAttribute("a", "   ");
            }
            catch (ArgumentException ex)
            {
                error = ex.Message.Contains("Claims cannot be null or empty or white space");
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresClaims_UnauthenticatedUser_ReturnsUnauthenticated()
        {
            var attr = new RequiresClaimsAttribute("a");
            var user = new ClaimsPrincipal();
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthenticated.SetResponseCode, shouldContinue.SetResponseCode);
        }
        
        [Fact]
        public void RequiresClaims_UnauthorizedUser_ReturnsUnauthorized()
        {
            var attr = new RequiresClaimsAttribute("a");
            var user = UserSetup.GetUser();
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthorized.SetResponseCode, shouldContinue.SetResponseCode);
        }
        
        [Fact]
        public void RequiresClaims_AuthorizedUser_ReturnsAuthorized()
        {
            var attr = new RequiresClaimsAttribute("a");
            var user = UserSetup.GetUserWithClaims("a:0");
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.True(shouldContinue.ShouldContinue);
        }
    }
}
