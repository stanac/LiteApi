using LiteApi.Contracts.Models;
using LiteApi.Services;
using Xunit;

namespace LiteApi.Tests
{
    public class PathResolverTests
    {
        [Fact]
        public void PathResolver_With_Zero_Controllers_Will_Return_Null()
        {
            PathResolver resolver = new PathResolver(new ControllerContext[0]);
            var action = resolver.ResolvePath(new Fakes.FakeHttpRequest { Path = "/test1/test2/test3" });
        }
    }
}
