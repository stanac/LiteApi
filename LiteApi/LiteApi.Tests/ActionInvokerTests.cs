using LiteApi.Contracts.Abstractions;
using LiteApi.Services;
using LiteApi.Services.ModelBinders;
using LiteApi.Tests.ModelSetup;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class ActionInvokerTests
    {
        [Fact]
        public void ActionInvoker_NullArguments_ThrowException()
        {
            bool error = false;
            try
            {
                var a = new ActionInvoker(null, new ModelBinderCollection(
                    new JsonSerializer(), Fakes.FakeServiceProvider.GetServiceProvider(), new Fakes.FakeDefaultLiteApiOptionsRetriever()), new JsonSerializer());
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);

            error = false;
            try
            {
                var a = new ActionInvoker(new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object), null, new JsonSerializer());
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public async Task ActionInvoker_UnautheticatedUser_CanSetMissingStatusCode()
        {
            await AssertNotSetResponseCode(new ClaimsPrincipal(), ApiFilterRunResult.Unauthenticated.SetResponseCode.Value);
        }

        [Fact]
        public async Task ActionInvoker_UnauthorizedUser_CanSetMissingStatusCode()
        {
            await AssertNotSetResponseCode(UserSetup.GetUser(), ApiFilterRunResult.Unauthorized.SetResponseCode.Value);
        }

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

        [Fact]
        public async Task ActionInvoker_MethodWithDefaultParam_CanBeInvoked()
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.ParametersController));
            var controller = discoverer.GetControllers(null).Single();
            IActionInvoker invoker = new ActionInvoker(
                new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object),
                new ModelBinderCollection(new JsonSerializer(), Fakes.FakeServiceProvider.GetServiceProvider(), new Fakes.FakeDefaultLiteApiOptionsRetriever())
                , new JsonSerializer());
            var ctx = new Fakes.FakeHttpContext();
            await invoker.Invoke(ctx, controller.Actions.First(x => x.Name == "toupper"), null);
            string body = ctx.Response.ReadBody();
            Assert.Equal("\"ABC\"", body);

            ctx = new Fakes.FakeHttpContext();
            (ctx.Request as Fakes.FakeHttpRequest).AddQuery("a", "zxc");
            await invoker.Invoke(ctx, controller.Actions.First(x => x.Name == "toupper"), null);
            body = ctx.Response.ReadBody();
            Assert.Equal("\"ZXC\"", body);
        }

        [Fact]
        public async Task ActionInvoker_InvokeVoid_CanInvoke()
        {
            await AssertResponseBody("VoidAction", "");
        }

        [Fact]
        public async Task ActionInvoker_InvokeTask_CanInvoke()
        {
            await AssertResponseBody("TaskAction", "");
        }

        [Fact]
        public async Task ActionInvoker_InvokeIntTask_CanInvoke()
        {
            await AssertResponseBody("IntTaskAction", "1");
        }

        [Fact]
        public async Task ActionInvoker_InvokeInt_CanInvoke()
        {
            await AssertResponseBody("IntAction", "1");
        }

        private async Task AssertResponseBody(string actionName, string expectedResult)
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.DifferentMethodTypesController));
            var controller = discoverer.GetControllers(null).Single();
            IActionInvoker invoker = new ActionInvoker(
                new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object),
                new ModelBinderCollection(new JsonSerializer(), Fakes.FakeServiceProvider.GetServiceProvider(), new Fakes.FakeDefaultLiteApiOptionsRetriever())
                , new JsonSerializer()
                );
            var ctx = new Fakes.FakeHttpContext();
            (ctx.Request as Fakes.FakeHttpRequest).AddQuery("a", "2").AddQuery("b", "3").AddQuery("c", "4").AddQuery("d", "5");
            await invoker.Invoke(ctx, controller.Actions.First(x => x.Name == actionName.ToLower()), null);
            string body = ctx.Response.ReadBody();
            Assert.Equal(expectedResult, body);
        }

        private async Task AssertResponseBody(Contracts.Models.SupportedHttpMethods actionMethod, string expectedResult)
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.DifferentHttpMethodsController));
            var controller = discoverer.GetControllers(null).Single();
            IActionInvoker invoker = new ActionInvoker(
                new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object),
                new ModelBinderCollection(new JsonSerializer(), Fakes.FakeServiceProvider.GetServiceProvider(), new Fakes.FakeDefaultLiteApiOptionsRetriever())
                , new JsonSerializer());
            var ctx = new Fakes.FakeHttpContext();
            (ctx.Request as Fakes.FakeHttpRequest).AddQuery("a", "2").AddQuery("b", "3").AddQuery("c", "4").AddQuery("d", "5");
            await invoker.Invoke(ctx, controller.Actions.First(x => x.HttpMethod == actionMethod), null);
            string body = ctx.Response.ReadBody();
            Assert.Equal(expectedResult, body);
        }

        private async Task AssertResponseHttpStatusCode(Contracts.Models.SupportedHttpMethods actionMethod, int expectedCode)
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.DifferentHttpMethodsController));
            var controller = discoverer.GetControllers(null).Single();
            IActionInvoker invoker = new ActionInvoker(
                new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object),
                new ModelBinderCollection(new JsonSerializer(), Fakes.FakeServiceProvider.GetServiceProvider(), new Fakes.FakeDefaultLiteApiOptionsRetriever())
                , new JsonSerializer()
                );
            var ctx = new Fakes.FakeHttpContext();
            ctx.Request.Method = actionMethod.ToString().ToLower();
            (ctx.Request as Fakes.FakeHttpRequest).AddQuery("a", "2").AddQuery("b", "3").AddQuery("c", "4").AddQuery("d", "5");
            await invoker.Invoke(ctx, controller.Actions.First(x => x.HttpMethod == actionMethod), null);
            Assert.Equal(expectedCode, ctx.Response.StatusCode);
        }

        private async Task AssertNotSetResponseCode(ClaimsPrincipal user, int expectedStatusCode)
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.SecureController2));
            var controller = discoverer.GetControllers(null).Single();
            IActionInvoker invoker = new ActionInvoker(
                new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object),
                new ModelBinderCollection(new JsonSerializer(), Fakes.FakeServiceProvider.GetServiceProvider(), new Fakes.FakeDefaultLiteApiOptionsRetriever())
                , new JsonSerializer());
            var httpCtx = new Fakes.FakeHttpContext();
            httpCtx.User = user;
            var action = controller.Actions.First(x => x.Name == "get14");
            await invoker.Invoke(httpCtx, action, null);
            int statusCode = httpCtx.Response.StatusCode;
            Assert.Equal(expectedStatusCode, statusCode);
        }
    }
}
