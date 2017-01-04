using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Tests.ModelSetup;
using System;
using System.Security.Claims;
using Xunit;

namespace LiteApi.Tests.FilterTests
{
    public class RequiresClaimWithValuesTests
    {
        [Fact]
        public void RequiresClaimWithValues_ClaimTypeNull_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresClaimWithValuesAttribute(null, "a", "b", "c");
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresClaimWithValues_ClaimTypeEmtpty_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresClaimWithValuesAttribute("", "a", "b", "c");
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresClaimWithValues_ClaimTypeWhiteSpace_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresClaimWithValuesAttribute("   ", "a", "b", "c");
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }
        
        [Fact]
        public void RequiresClaimWithValues_ClaimValuesNull_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresClaimWithValuesAttribute("a", null);
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }
        
        [Fact]
        public void RequiresClaimWithValues_ClaimValuesEmptyString_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresClaimWithValuesAttribute("a", "", "a", "b");
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresClaimWithValues_ClaimValuesContainsNull_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresClaimWithValuesAttribute("a", "a", "b", null);
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresClaimWithValues_ClaimValuesContainsWhiteSpace_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresClaimWithValuesAttribute("a", "a", "b", "  ");
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresClaimWithValues_UnauthenticatedUser_ReturnsUnauthenticated()
        {
            var attr = new RequiresClaimWithValuesAttribute("a", "1", "2", "3");
            var user = new ClaimsPrincipal();
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthenticated.SetResponseCode, shouldContinue.SetResponseCode);
        }

        [Fact]
        public void RequiresClaimWithValues_UserWithoutClaims_ReturnsUnauthorized()
        {
            var attr = new RequiresClaimWithValuesAttribute("a", "1", "2", "3");
            var user = UserSetup.GetUser();
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthorized.SetResponseCode, shouldContinue.SetResponseCode);
        }

        [Fact]
        public void RequiresClaimWithValues_UserClaimWithSomeValues_ReturnsUnauthorized()
        {
            var attr = new RequiresClaimWithValuesAttribute("a", "1", "2", "3");
            var user = UserSetup.GetUserWithClaims("a:1", "a:2");
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthorized.SetResponseCode, shouldContinue.SetResponseCode);
        }

        [Fact]
        public void RequiresClaimWithValues_UserClaimWithAllValues_ReturnsContinue()
        {
            var attr = new RequiresClaimWithValuesAttribute("a", "1", "2", "3");
            var user = UserSetup.GetUserWithClaims("a:1", "a:2", "a:3");
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.True(shouldContinue.ShouldContinue);
        }
    }
}
