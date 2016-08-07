using LiteApi.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Services
{
    public class ControllersValidator : IControllersValidator
    {
        private readonly IActionsValidator _actionValidator;

        public ControllersValidator(IActionsValidator actionValidator)
        {
            if (actionValidator == null) throw new ArgumentNullException(nameof(actionValidator));
            _actionValidator = actionValidator;
        }

        public IEnumerable<string> GetValidationErrors(ControllerContext[] controllerCtxs)
        {
            foreach (ControllerContext ctrl in controllerCtxs)
            {
                if (controllerCtxs.Count(x => x.Name == ctrl.Name) > 2)
                {
                    yield return $"There are more than one controller with matching name: {ctrl.Name}";
                }
                foreach (string error in _actionValidator.GetValidationErrors(ctrl.Actions))
                {
                    yield return error;
                }
            }
        }
    }
}
