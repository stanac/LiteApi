using LiteApi.Services;
using System.Linq;
using Xunit;

namespace LiteApi.Tests
{
    public class ValidatorsTests
    {
        [Fact]
        public void Validators_TwoControllersWithSameName_ReturnsError()
        {
            AssertErrorMessage("There are more than one controller with matching name: api/two");
        }

        [Fact]
        public void Validators_GetActionWithParameters_ReturnsError()
        {
            AssertErrorMessage("parameter from body found. in action 'getthevalue' in controller: 'api/two");
        }

        [Fact]
        public void Validators_DeleteActionWithParameters_ReturnsError()
        {
            AssertErrorMessage("parameter from body found. in action 'deletethevalue' in controller: 'api/two");
        }

        [Fact]
        public void Validators_ActionWithTwoParametersFromBody_ReturnsError()
        {
            AssertErrorMessage(" Multiple parameters from body found in action 'postint' in controller 'api/two'");
        }

        [Fact]
        public void Validators_ActionWithZeroSegments_ReturnsError()
        {
            AssertErrorMessage("Action (Int32 NoSegments()) in controller api/two(LiteApi.Contracts.Models.ControllerContext) has 0 constant route segments");
        }
        
        [Fact]
        public void Validators_ActionWithZeroConstantSegments_ReturnsError()
        {
            AssertErrorMessage("Action (Int32 NoConstantSegments(Int32, Int32, Int32)) in controller api/two(LiteApi.Contracts.Models.ControllerContext) has 0 constant route segments");
        }

        [Fact]
        public void Validators_GenericArrayAndListParameters_AreAcceptable()
        {
            var discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.CollectionController));
            var ctrls = discoverer.GetControllers(null);
            var validator = new ControllersValidator(new ActionsValidator(new ParametersValidator()));
            var errors = validator.GetValidationErrors(ctrls).ToArray();
            Assert.Empty(errors);
        }

        [Fact]
        public void Validators_GenericArrayAndListWithComplexParameters_AreNotAcceptable()
        {
            var discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.InvalidCollectionsController));
            var ctrls = discoverer.GetControllers(null);
            var validator = new ControllersValidator(new ActionsValidator(new ParametersValidator()));
            var errors = validator.GetValidationErrors(ctrls).ToArray();
            Assert.Equal(3, errors.Length);
            Assert.True(errors.Any(x => x.Contains("InvalidCollectionsGet1".ToLower())));
            Assert.True(errors.Any(x => x.Contains("InvalidCollectionsGet2".ToLower())));
            Assert.True(errors.Any(x => x.Contains("InvalidCollectionsGet3".ToLower())));
        }
        
        private void AssertErrorMessage(string errorMsg)
        {
            var discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.Two), typeof(Controllers.TwoController));
            var ctrls = discoverer.GetControllers(null);
            var validator = new ControllersValidator(new ActionsValidator(new ParametersValidator()));
            var errors = validator.GetValidationErrors(ctrls).ToArray();
            Assert.True(errors.Any(x => x.Contains(errorMsg)));
        }
    }
}
