using LiteApi.Services;
using LiteApi.Services.ModelBinders;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace LiteApi.Tests
{
    public class RawResponseTests
    {
        [Fact]
        public void CanReturnRawJson()
        {
            var response = InvokeAndGetResponse();
            Assert.Equal("{ \"a\": \"b\" }", response.ReadBody());
            Assert.Equal("application/json", response.ContentType);
        }

        [Fact]
        public void CanReturnCustomHeader()
        {
            var response = InvokeAndGetResponse();
            Assert.Equal("val", response.Headers["key"].First());
        }

        [Fact]
        public void CanReturnCustomStatus()
        {
            var response = InvokeAndGetResponse();
            Assert.Equal(241, response.StatusCode);
        }

        private HttpResponse InvokeAndGetResponse()
        {
            var ctrl = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.RawController))
                .GetControllers(null)[0];
            var action = ctrl.Actions[0];
            var serviceProvider = Fakes.FakeServiceProvider.GetServiceProvider();
            var ctrlBuilder = new ControllerBuilder(serviceProvider);
            var actionInvoker = new ActionInvoker(ctrlBuilder, new ModelBinderCollection(new JsonSerializer(), serviceProvider), new JsonSerializer());
            var httpCtx = new Fakes.FakeHttpContext();
            actionInvoker.Invoke(httpCtx, action).Wait();
            return httpCtx.Response;
        }
    }
}
