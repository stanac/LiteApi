using LiteApi.Contracts.Abstractions;
using LiteApi.Tests.ModelSetup;
using System;
using System.Security.Claims;
using Xunit;

namespace LiteApi.Tests.FilterTests
{
    public class RequiresRolesTests
    {
        [Fact]
        public void RequiresRoles_NullRoles_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresRolesAttribute(null);
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresRoles_RolesAreEmptyArray_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresRolesAttribute();
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresRoles_RolesContainsNull_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresRolesAttribute("a", null);
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresRoles_RolesContainsEmptyString_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresRolesAttribute("a", "");
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresRoles_RolesContainsWhiteSpaceString_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresRolesAttribute("a", " ");
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresRoles_UnauthenticatedUser_ReturnsUnauthenticated()
        {
            var attr = new RequiresRolesAttribute("a", "b");
            var user = new ClaimsPrincipal();
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthenticated.SetResponseCode, shouldContinue.SetResponseCode);
        }

        [Fact]
        public void RequiresRoles_UserWithoutRoles_ReturnsUnauthorized()
        {
            var attr = new RequiresRolesAttribute("a", "b");
            var user = UserSetup.GetUser();
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthorized.SetResponseCode, shouldContinue.SetResponseCode);
        }

        [Fact]
        public void RequiresRoles_UserWitSomeRoles_ReturnsUnauthorized()
        {
            var attr = new RequiresRolesAttribute("a", "b");
            var user = UserSetup.GetUser("a");
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthorized.SetResponseCode, shouldContinue.SetResponseCode);
        }

        [Fact]
        public void RequiresRoles_UserWitAllRoles_ReturnsContinue()
        {
            var attr = new RequiresRolesAttribute("a", "b");
            var user = UserSetup.GetUser("a", "b");
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.True(shouldContinue.ShouldContinue);
        }
    }
}
