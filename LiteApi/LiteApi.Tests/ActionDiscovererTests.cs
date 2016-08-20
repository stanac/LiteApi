using LiteApi.Services;
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
                Name = "TestController",
                ControllerType = typeof(Controllers.ActionOverloadController)
            });

            Assert.True(actions.Length > 0);
        }

        private ActionDiscoverer GetActionDiscoverer() => new ActionDiscoverer(new Fakes.FakeParametersDiscoverer());
        
    }
}
