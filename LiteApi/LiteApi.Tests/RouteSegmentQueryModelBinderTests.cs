using LiteApi.Contracts.Models;
using LiteApi.Services.ModelBinders;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Xunit;

namespace LiteApi.Tests
{
    public class RouteSegmentQueryModelBinderTests
    {
        [Fact]
        public void RouteSegmentQueryModelBinder_GetParameterValueWithNullParameter_ThrowsException()
        {
            var request = GetRequest();
            var actionCtx = GetActionContext();
            var param = actionCtx.Parameters.First();

            AssertException(false, actionCtx, param, request);
            AssertException(true, null, param, request);
            AssertException(true, actionCtx, null, request);
            AssertException(true, actionCtx, param, null);
        }

        [Fact]
        public void RouteSegmentQueryModelBinder_GetParameterValueParamSegmentMissing_ThrowsException()
        {
            var request = GetRequest("/api/v2/route/1/plus");
            var actionCtx = GetActionContext();
            var param = actionCtx.Parameters.First(x => x.Name == "b");
            bool error = false;
            try
            {
                var a = RouteSegmentModelBinder.GetParameterValue(actionCtx, param, request);
            }
            catch (Exception ex)
            {
                error = ex.Message.Contains("Route segment for parameter");
            }

            Assert.True(error);
        }

        private void AssertException(bool expectsArgumentNullException, ActionContext actionCtx, ActionParameter param, HttpRequest request)
        {
            bool error = false;
            try
            {
                var a = RouteSegmentModelBinder.GetParameterValue(actionCtx, param, request);
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.Equal(expectsArgumentNullException, error);
        }


        private HttpRequest GetRequest(string path = null)
        {
            var request = Fakes.FakeHttpRequest.WithGetMethod();
            request.Path = path ?? "/api/v2/route/1/plus/2";
            return request;
        }

        private ActionContext GetActionContext()
        {
            var ctrlDiscoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.RouteParamsController));
            return ctrlDiscoverer.GetControllers(null).Single().Actions.First();

        }
    }
}