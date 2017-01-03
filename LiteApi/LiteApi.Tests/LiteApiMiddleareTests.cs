using System;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class LiteApiMiddleareTests
    {
        [Fact]
        public async Task LiteApiMiddleareTests_Registered_CanBeInvoked()
        {
            var middleware = new LiteApiMiddleware(null, LiteApiOptions.Default, null);
            var httpCtx = new Fakes.FakeHttpContext();
            httpCtx.Request.Method = "GET";
            httpCtx.Request.Path = "/";
            await middleware.Invoke(httpCtx);
            

            // expect exception on next registration
            TestExtensions.AssertExpectedException<Exception>(() =>
                new LiteApiMiddleware(null, LiteApiOptions.Default, null),
                "Middleware can be registered twice");
        }
    }
}
