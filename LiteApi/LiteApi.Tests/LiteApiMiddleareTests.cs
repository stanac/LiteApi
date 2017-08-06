using System;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class LiteApiMiddleareTests
    {
        [Fact]
        public void LiteApiMiddleareTests_Registered_CanBeInvoked()
        {
            lock (TestLock.Lock)
            {
                var servicesMock = new Moq.Mock<IServiceProvider>();
                servicesMock.Setup(x => x.GetService(typeof(IServiceProvider))).Returns(servicesMock.Object);

                var middleware = new LiteApiMiddleware(null, LiteApiOptions.Default, servicesMock.Object);
                var httpCtx = new Fakes.FakeHttpContext();
                httpCtx.Request.Method = "GET";
                httpCtx.Request.Path = "/";
                middleware.Invoke(httpCtx).Wait();
                
                // expect exception on next registration
                TestExtensions.AssertExpectedException<Exception>(() =>
                    new LiteApiMiddleware(null, LiteApiOptions.Default, null),
                    "Middleware can be registered twice");
            }
        }
    }
}
