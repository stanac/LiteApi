using LiteApi.Contracts.Abstractions;
using LiteApi.Tests.ModelSetup;
using System.Security.Claims;
using Xunit;

namespace LiteApi.Tests.FilterTests
{
    public class RequiresAuthenticationTests
    {
        [Fact]
        public void RequiresAuthentication_UnauthenticatedUser_ReturnsUnauthenticated()
        {
            var user = new ClaimsPrincipal();
            var attr = new RequiresAuthenticationAttribute();
            var httpCtx = new Fakes.FakeHttpContext();
            httpCtx.User = user;
            var shouldContinue = attr.ShouldContinue(httpCtx);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthenticated.SetResponseCode, shouldContinue.SetResponseCode);
        }

        [Fact]
        public void RequiresAuthentication_NullUser_ReturnsUnauthenticated()
        {
            var attr = new RequiresAuthenticationAttribute();
            var httpCtx = new Fakes.FakeHttpContext();
            var shouldContinue = attr.ShouldContinue(httpCtx);
            Assert.False(shouldContinue.ShouldContinue);
            Assert.Equal(ApiFilterRunResult.Unauthenticated.SetResponseCode, shouldContinue.SetResponseCode);
        }

        [Fact]
        public void RequiresAuthentication_AuthenticatedUser_ReturnsContinue()
        {
            var attr = new RequiresAuthenticationAttribute();
            var httpCtx = new Fakes.FakeHttpContext();
            httpCtx.User = UserSetup.GetUser();
            var shouldContinue = attr.ShouldContinue(httpCtx);
            Assert.True(shouldContinue.ShouldContinue);
        }
    }
}
