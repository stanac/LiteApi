using LiteApi.Services;
using LiteApi.Services.Validators;
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
        public void Validators_ActionParameterFromRouteWithDefaultValue_ReturnsError()
        {
            AssertErrorMessage("in controller api/two (LiteApi.Contracts.Models.ControllerContext) is from route and has default value.");
        }

        [Fact]
        public void Validators_ActionParameterFromRouteWithWithoutRouteSegment_ReturnsError()
        {
            AssertErrorMessage("Parameter b in action routeparamwithoutsegment (Int32 RouteParamWithoutSegment(Int32) in controller api/two (LiteApi.Contracts.Models.ControllerContext) is from route and there is no matching route segment found");
        }

        [Fact]
        public void Validators_ActionRouteParameterSegmentWihtoutParameter_ReturnsError()
        {
            AssertErrorMessage("Route segment {b} is set as parameter without matching parameter in method");
        }

        [Fact]
        public void Validators_ControllerWithNotRegisteredAuthPolicy_ReturnsError()
        {
            AssertErrorMessage("Authorization policy NonExistingPolicy2 is defined on controller api/two but");
        }
        
        [Fact]
        public void Validators_ActionWithNotRegisteredAuthPolicy_ReturnsError()
        {
            AssertErrorMessage("has defined authorization policy NonExistingPolicy which is not registered within middleware");
        }

        [Fact]
        public void Validators_GenericArrayAndListParameters_AreAcceptable()
        {
            var discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.CollectionController));
            var ctrls = discoverer.GetControllers(null);
            var validator = GetCtrlValidator();
            var errors = validator.GetValidationErrors(ctrls).ToArray();
            Assert.Empty(errors);
        }

        [Fact]
        public void Validators_GenericArrayAndListWithComplexParameters_AreNotAcceptable()
        {
            var discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.InvalidCollectionsController));
            var ctrls = discoverer.GetControllers(null);
            var validator = GetCtrlValidator();
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
            var validator = GetCtrlValidator();
            var errors = validator.GetValidationErrors(ctrls).ToArray();
            Assert.True(errors.Any(x => x.Contains(errorMsg)));
        }

        private ControllersValidator GetCtrlValidator()
        {
            var store = new AuthorizationPolicyStore();
            var paramValidator = new ParametersValidator();
            var actionValidator = new ActionsValidator(paramValidator, store);
            return new ControllersValidator(actionValidator, store);
        }
    }
}
