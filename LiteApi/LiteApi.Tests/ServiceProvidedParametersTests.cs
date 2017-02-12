using LiteApi.Contracts.Abstractions;
using LiteApi.Services;
using LiteApi.Services.ModelBinders;
using Moq;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class ServiceProvidedParametersTests
    {
        [Fact]
        public async Task CanGetParameterFromService()
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.ServiceProvidedParameterController));
            var ctrl = discoverer.GetControllers(null).Single();
            var action = ctrl.Actions.Single();
            var mb = new ModelBinderCollection(new JsonSerializer(), GetServiceProvider());
            var httpCtx = new Fakes.FakeHttpContext();
            (httpCtx.Request as Fakes.FakeHttpRequest).AddQuery("i", "1");
            object[] parameters = mb.GetParameterValues(httpCtx.Request, action);
            Assert.Equal(2, parameters.Length);
            Assert.True(typeof(Controllers.IIncrementService).IsAssignableFrom(parameters[1].GetType()));

            ActionInvoker invoker = new ActionInvoker(new ControllerBuilder(GetServiceProvider()), mb);
            await invoker.Invoke(httpCtx, action);
            var result = httpCtx.Response.ReadBody();
            Assert.Equal("2", result);
        }

        private IServiceProvider GetServiceProvider()
        {
            var mock = new Mock<IServiceProvider>();
            mock.Setup(x => x.GetService(It.IsAny<Type>())).Returns(new Func<Type, object>(
                type =>
                {
                    if (type == typeof(Controllers.IIncrementService))
                        return new Controllers.IncrementService();
                    return null;
                }));
            return mock.Object;
        }
    }
}
