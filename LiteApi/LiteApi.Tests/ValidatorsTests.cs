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
            AssertErrorMessage("There are more than one controller with matching name: two");
        }

        [Fact]
        public void Validators_GetActionWithParameters_ReturnsError()
        {
            AssertErrorMessage("parameter from body found. in action 'getthevalue' in controller: 'two");
        }

        [Fact]
        public void Validators_DeleteActionWithParameters_ReturnsError()
        {
            AssertErrorMessage("parameter from body found. in action 'deletethevalue' in controller: 'two");
        }

        [Fact]
        public void Validators_ActionWithTwoParametersFromBody_ReturnsError()
        {
            AssertErrorMessage(" Multiple parameters from body found in action 'postint' in controller 'two'");
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
