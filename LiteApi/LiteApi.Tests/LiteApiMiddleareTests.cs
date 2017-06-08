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
            var servicesMock = new Moq.Mock<IServiceProvider>();
            servicesMock.Setup(x => x.GetService(typeof(IServiceProvider))).Returns(servicesMock.Object);

            var middleware = new LiteApiMiddleware(null, LiteApiOptions.Default, servicesMock.Object);
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
