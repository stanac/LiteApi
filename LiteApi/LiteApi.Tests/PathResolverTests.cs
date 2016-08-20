using LiteApi.Contracts.Models;
using LiteApi.Services;
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
            resolver.ResolveAndAssert(request, "default get");

            request.AddQuery("a", "5");
            resolver.ResolveAndAssert(request, "get");

            request.Method = "POST";
            resolver.ResolveAndAssert(request, "post");
            
            request.Method = "pUT";
            resolver.ResolveAndAssert(request, "put");

            request.Method = "DelETe";
            resolver.ResolveAndAssert(request, "delete");
        }

        [Fact]
        public void PathResolver_ControllerWithOverridenActions_CanResolveMethodOverridenWithDifferentNumberOfParameters()
        {
            var ctrlCtx = new Controllers.ActionOverloadController().GetControllerContextAsArray();

            var resolver = new PathResolver(ctrlCtx);
            var path = "/api/ActionOverload/GetString";
            var request = Fakes.FakeHttpRequest.WithGetMethod()
                .WithPath(path)
                .AddQuery("a", "test string");

            resolver.ResolveAndAssert(request, "string a");
        }
                
    }
}
