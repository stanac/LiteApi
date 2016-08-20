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
            var action = resolver.ResolvePath(new Fakes.FakeHttpRequest { Path = "/test1/test2/test3" });
        }

        [Fact]
        public void PathResolver_ControllerWithDifferentHttpMethods_CanResolveMethodByHttpMethod()
        {
            var ctrlCtx = new Controllers.DifferentHttpMethodsController().GetControllerContextAsArray();

            var resolver = new PathResolver(ctrlCtx);
            // resolver.ResolvePath
            throw new System.NotImplementedException();
        }
        
        [Fact]
        public void PathResolver_ControllerWithOverridenActions_CanResolveMethodOverridenWithDifferentNumberOfParameters()
        {
            throw new System.NotImplementedException();
        }
    }
}
