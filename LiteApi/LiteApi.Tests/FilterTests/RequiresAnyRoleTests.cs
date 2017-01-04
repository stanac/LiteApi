using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Tests.ModelSetup;
using System;
using System.Security.Claims;
using Xunit;

namespace LiteApi.Tests.FilterTests
{
    public class RequiresAnyRoleTests
    {
        [Fact]
        public void RequiresAnyRole_NullRoles_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresAnyRoleAttribute(null);
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresAnyRole_RolesAreEmptyArray_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresAnyRoleAttribute();
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresAnyRole_RolesContainsNull_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresAnyRoleAttribute("a", null);
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresAnyRole_RolesContainsEmptyString_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresAnyRoleAttribute("a", "");
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresAnyRole_RolesContainsWhiteSpaceString_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new RequiresAnyRoleAttribute("a", " ");
            }
            catch (ArgumentException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void RequiresAnyRole_UnauthenticatedUser_ReturnsUnauthenticated()
        {
            var attr = new RequiresAnyRoleAttribute("a", "b");
            var user = new ClaimsPrincipal();
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthenticated.SetResponseCode, shouldContinue.SetResponseCode);
        }

        [Fact]
        public void RequiresAnyRole_UserWithoutRoles_ReturnsUnauthorized()
        {
            var attr = new RequiresAnyRoleAttribute("a", "b");
            var user = UserSetup.GetUser();
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthorized.SetResponseCode, shouldContinue.SetResponseCode);
        }

        [Fact]
        public void RequiresAnyRole_UserWitSomeRoles_ReturnsContinue()
        {
            var attr = new RequiresAnyRoleAttribute("a", "b");
            var user = UserSetup.GetUser("a");
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.True(shouldContinue.ShouldContinue);
        }

        [Fact]
        public void RequiresAnyRole_UserWitAllRoles_ReturnsContinue()
        {
            var attr = new RequiresAnyRoleAttribute("a", "b");
            var user = UserSetup.GetUser("a", "b");
            var httpContext = new Fakes.FakeHttpContext();
            httpContext.User = user;
            var shouldContinue = attr.ShouldContinue(httpContext);
            Assert.True(shouldContinue.ShouldContinue);
        }
    }
}
