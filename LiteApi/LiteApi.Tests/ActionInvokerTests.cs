using LiteApi.Contracts.Abstractions;
using LiteApi.Services;
using LiteApi.Services.ModelBinders;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class ActionInvokerTests
    {
        [Fact]
        public async Task ActionInvoker_GetMethod_ReturnsResult()
        {
            await AssertResponseBody(Contracts.Models.SupportedHttpMethods.Get, "\"default get\"");   
        }

        [Fact]
        public async Task ActionInvoker_PostMethod_ReturnsResult()
        {
            await AssertResponseBody(Contracts.Models.SupportedHttpMethods.Post, "\"post\"");
        }

        [Fact]
        public async Task ActionInvoker_PutMethod_ReturnsResult()
        {
            await AssertResponseBody(Contracts.Models.SupportedHttpMethods.Put, "\"put\"");
        }

        [Fact]
        public async Task ActionInvoker_DeleteMethod_ReturnsResult()
        {
            await AssertResponseBody(Contracts.Models.SupportedHttpMethods.Delete, "\"delete\"");
        }

        [Fact]
        public async Task ActionInvoker_GetMethod_ReturnsStatus200()
        {
            await AssertResponseHttpStatusCode(Contracts.Models.SupportedHttpMethods.Get, 200);
        }

        [Fact]
        public async Task ActionInvoker_PostMethod_ReturnsStatus201()
        {
            await AssertResponseHttpStatusCode(Contracts.Models.SupportedHttpMethods.Post, 201);
        }

        [Fact]
        public async Task ActionInvoker_PutMethod_ReturnsStatus201()
        {
            await AssertResponseHttpStatusCode(Contracts.Models.SupportedHttpMethods.Put, 201);
        }

        [Fact]
        public async Task ActionInvoker_DeleteMethod_ReturnsStatus204()
        {
            await AssertResponseHttpStatusCode(Contracts.Models.SupportedHttpMethods.Delete, 204);
        }

        private async Task AssertResponseBody(Contracts.Models.SupportedHttpMethods actionMethod, string expectedResult)
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.DifferentHttpMethodsController));
            var controller = discoverer.GetControllers(null).Single();
            IActionInvoker invoker = new ActionInvoker(
                new ControllerBuilder(),
                new ModelBinderCollection(new JsonSerializer())
                );
            var ctx = new Fakes.FakeHttpContext();
            (ctx.Request as Fakes.FakeHttpRequest).AddQuery("a", "2").AddQuery("b", "3").AddQuery("c", "4").AddQuery("d", "5");
            await invoker.Invoke(ctx, controller.Actions.First(x => x.HttpMethod == actionMethod));
            string body = ctx.Response.ReadBody();
            Assert.Equal(expectedResult, body);
        }

        private async Task AssertResponseHttpStatusCode(Contracts.Models.SupportedHttpMethods actionMethod, int expectedCode)
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.DifferentHttpMethodsController));
            var controller = discoverer.GetControllers(null).Single();
            IActionInvoker invoker = new ActionInvoker(
                new ControllerBuilder(),
                new ModelBinderCollection(new JsonSerializer())
                );
            var ctx = new Fakes.FakeHttpContext();
            ctx.Request.Method = actionMethod.ToString().ToLower();
            (ctx.Request as Fakes.FakeHttpRequest).AddQuery("a", "2").AddQuery("b", "3").AddQuery("c", "4").AddQuery("d", "5");
            await invoker.Invoke(ctx, controller.Actions.First(x => x.HttpMethod == actionMethod));
            Assert.Equal(expectedCode, ctx.Response.StatusCode);
        }
    }
}
