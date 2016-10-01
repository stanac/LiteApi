using LiteApi.Services;
using LiteApi.Services.Discoverers;
using System.Linq;
using Xunit;

namespace LiteApi.Tests
{
    public class ActionDiscovererTests
    {
        [Fact]
        public void ActionDiscoverer_CanDiscoverActions()
        {
            var discoverer = GetActionDiscoverer();
            var actions = discoverer.GetActions(new Contracts.Models.ControllerContext
            {
                RouteAndName = "TestController",
                ControllerType = typeof(Controllers.ActionOverloadController)
            });

            Assert.True(actions.Length > 0);
        }

        [Fact]
        public void ActionDiscoverer_ToStringMethod_WontBeDiscover()
        {
            var discoverer = GetActionDiscoverer();
            var actions = discoverer.GetActions(new Contracts.Models.ControllerContext
            {
                RouteAndName = "TestController",
                ControllerType = typeof(Controllers.ActionOverloadController)
            });

            var toStringAction = actions.FirstOrDefault(x => x.Name == "tostring");
            Assert.Null(toStringAction);
        }

        [Fact]
        public void ActionDiscoverer_MethodWithDontMapToApiAttribute_WontBeDiscovered()
        {
            var discoverer = GetActionDiscoverer();
            var actions = discoverer.GetActions(new Contracts.Models.ControllerContext
            {
                RouteAndName = "TestController",
                ControllerType = typeof(Controllers.ActionOverloadController)
            });

            var toStringAction = actions.FirstOrDefault(x => x.Name == "NotMappedMethod".ToLower());
            Assert.Null(toStringAction);
        }


        private ActionDiscoverer GetActionDiscoverer() => new ActionDiscoverer(new Fakes.FakeParametersDiscoverer());
        
    }
}
