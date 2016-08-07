using LiteApi.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi.Services
{
    public class ActionsValidator : IActionsValidator
    {
        private readonly IParametersValidator _paramsValidator;

        public ActionsValidator(IParametersValidator paramsValidator)
        {
            if (paramsValidator == null) throw new ArgumentNullException(nameof(paramsValidator));
            _paramsValidator = paramsValidator;
        }

        public IEnumerable<string> GetValidationErrors(ActionContext[] actionCtxs)
        {
            foreach (var action in actionCtxs)
            {
                if (actionCtxs.Count(x => x.Name == action.Name && x.HttpMethod == action.HttpMethod) > 1)
                {
                    yield return $"In controller {action.ParentController.Name} there are multiple {action.HttpMethod} actions with name {action.Name}. Method overriding is not supported, use optional parameters.";
                }
                foreach (var error in _paramsValidator.GetParametersErrors(action))
                {
                    yield return $"Error with parameters in controller {action.ParentController.Name} action {action.Name}: {error}";
                }
            }
        }
    }
}
