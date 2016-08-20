using LiteApi.Contracts.Models;
using LiteApi.Services;
using System.Reflection;
using Xunit;

namespace LiteApi.Tests
{
    public class PathResolverTests
    {
        [Fact]
        public void PathResolver_ZeroControllers_ReturnsNull()
        {
            PathResolver resolver = new PathResolver(new ControllerContext[0]);
            var request = Fakes.FakeHttpRequest.WithGetMethod().WithPath("/test1/test2/test3");
            var action = resolver.ResolveAction(request);
            Assert.Null(action);
        }

        [Fact]
        public void PathResolver_ControllerWithDifferentHttpMethods_CanResolveMethodByHttpMethod()
        {
            var ctrlCtx = new Controllers.DifferentHttpMethodsController().GetControllerContextAsArray();

            var resolver = new PathResolver(ctrlCtx);
            var path = "/api/DifferentHttpMethods/Action";
            var request = Fakes.FakeHttpRequest.WithGetMethod().WithPath(path);
            var getAction = resolver.ResolveAction(request);
            var actionResult = getAction.InvokeStringMethod();

            Assert.Equal("default get", actionResult);

            request.AddQuery("a", "5");
            getAction = resolver.ResolveAction(request);
            actionResult = getAction.InvokeStringMethod();

            Assert.Equal("get", actionResult);

            request.Method = "POST";
            var postAction = resolver.ResolveAction(request);
            actionResult = postAction.InvokeStringMethod();

            Assert.Equal("post", actionResult);


            request.Method = "pUT";
            var putAction = resolver.ResolveAction(request);
            actionResult = putAction.InvokeStringMethod();

            Assert.Equal("put", actionResult);


            request.Method = "DelETe";
            var deleteAction = resolver.ResolveAction(request);
            actionResult = deleteAction.InvokeStringMethod();

            Assert.Equal("delete", actionResult);
        }

        [Fact]
        public void PathResolver_ControllerWithOverridenActions_CanResolveMethodOverridenWithDifferentNumberOfParameters()
        {
            throw new System.NotImplementedException();
        }

        
    }
}
