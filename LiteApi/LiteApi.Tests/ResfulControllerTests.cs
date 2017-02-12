using LiteApi.Contracts.Abstractions;
using LiteApi.Services;
using LiteApi.Services.Discoverers;
using LiteApi.Services.ModelBinders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class ResfulControllerTests
    {
        // THERE IS AN ISSUE WITH xUnit, multiple inline data does not work
        
        [Theory]
        [InlineData(true,  "/api/restful/",  "",     "",  "[1,2,3]")]
        public async Task Controller_WithRestfulLinksAttributeNoQueryNoRoute_CanInvoke(bool isGet, string path, string query, string body, string expectedResult)
        {
            await AssertCall(isGet, path, query, body, expectedResult);
        }

        [Theory]
        [InlineData(true, "/api/restful/3", "", "", "5")]
        public async Task Controller_WithRestfulLinksAttributeNoQueryWithRoute_CanInvoke(bool isGet, string path, string query, string body, string expectedResult)
        {
            await AssertCall(isGet, path, query, body, expectedResult);
        }

        [Theory]
        [InlineData(true, "/api/restful", "id:2", "", "3")]
        public async Task Controller_WithRestfulLinksAttributeWithQuery_CanInvoke(bool isGet, string path, string query, string body, string expectedResult)
        {
            await AssertCall(isGet, path, query, body, expectedResult);
        }

        [Theory]
        [InlineData(false, "/api/restful", "", "4", "3")]
        public async Task Controller_WithRestfulLinksAttributePostWithoutRoot_CanInvoke(bool isGet, string path, string query, string body, string expectedResult)
        {
            await AssertCall(isGet, path, query, body, expectedResult);
        }

        [Theory]
        [InlineData(false, "/api/restful/4", "", "", "2")]
        public async Task Controller_WithRestfulLinksAttributePostWithRoot_CanInvoke(bool isGet, string path, string query, string body, string expectedResult)
        {
            await AssertCall(isGet, path, query, body, expectedResult);
        }


        private async Task AssertCall(bool isGet, string path, string query, string body, string expectedResult)
        {
            var ctrlDiscoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.RestfulController));
            var ctrls = ctrlDiscoverer.GetControllers(null);

            var pathResolver = new PathResolver(ctrls);
            var httpCtx = new Fakes.FakeHttpContext();
            if (isGet)
            {
                (httpCtx.Request as Fakes.FakeHttpRequest).WithPath(path);
            }
            else
            {
                (httpCtx.Request as Fakes.FakeHttpRequest).Method = "POST";
                (httpCtx.Request as Fakes.FakeHttpRequest).WithPath(path);
                (httpCtx.Request as Fakes.FakeHttpRequest).WriteBody(body);
            }
            foreach (var q in query.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                var parts = q.Split(":".ToCharArray());
                (httpCtx.Request as Fakes.FakeHttpRequest).AddQuery(parts[0], parts[1]);
            }

            var action = pathResolver.ResolveAction(httpCtx.Request);
            var actionInvoker = new ActionInvoker(new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object), new ModelBinderCollection(new JsonSerializer(), new Moq.Mock<IServiceProvider>().Object));
            await actionInvoker.Invoke(httpCtx, action);
            string result = httpCtx.Response.ReadBody();

            Assert.Equal(expectedResult, result);
        }

        private IActionInvoker GetActionInvoker()
        {
            return new ActionInvoker(
                new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object),
                new ModelBinderCollection(new JsonSerializer(), new Moq.Mock<IServiceProvider>().Object)
                );
        }
    }
}
