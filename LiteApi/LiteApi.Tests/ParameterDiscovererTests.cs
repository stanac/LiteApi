using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
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
        
        [Fact]
        public void ParameterDiscoverer_ImplicitParameterFromRoute_CanBeDiscovered()
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.RouteParamsController));
            var controller = discoverer.GetControllers(null).Single();
            var action = controller.Actions.Single(x => x.Name == "plus");
            Assert.Equal(3, action.Parameters.Length);
            var a = action.Parameters.First(x => x.Name == "a");
            var b = action.Parameters.First(x => x.Name == "b");
            var c = action.Parameters.First(x => x.Name == "c");

            Assert.Equal(ParameterSources.RouteSegment, a.ParameterSource);
            Assert.Equal(ParameterSources.RouteSegment, b.ParameterSource);
            Assert.Equal(ParameterSources.Query, c.ParameterSource);
        }

        [Fact]
        public void ParameterDiscoverer_ExplicitParameterFromRoute_CanBeDiscovered()
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.RouteParamsController));
            var controller = discoverer.GetControllers(null).Single();
            var action = controller.Actions.Single(x => x.Name == "plus2");
            Assert.Equal(3, action.Parameters.Length);
            var a = action.Parameters.First(x => x.Name == "a");
            var b = action.Parameters.First(x => x.Name == "b");
            var c = action.Parameters.First(x => x.Name == "c");

            Assert.Equal(ParameterSources.RouteSegment, a.ParameterSource);
            Assert.Equal(ParameterSources.RouteSegment, b.ParameterSource);
            Assert.Equal(ParameterSources.Query, c.ParameterSource);
        }
    }
}
