using LiteApi.Contracts.Abstractions;
using System.Linq;
using Xunit;

namespace LiteApi.Tests
{
    public class ParameterDiscovererTests
    {
        [Fact]
        public void ParameterDiscoverer_ParameterWithDefaultValue_CanBeDiscovered()
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.ParametersController));
            var controller = discoverer.GetControllers(null).Single();
            var action = controller.Actions.Single(x => x.Name == "toupper");
            Assert.True(action.Parameters.Single().HasDefaultValue);
            Assert.Equal("abc", action.Parameters.Single().DefaultValue);
        }
    }
}
