using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
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
                foreach (var error in _paramsValidator.GetParametersErrors(action))
                {
                    yield return $"Error with parameters in controller {action.ParentController.Name} action {action.Name}: {error}";
                }
            }
        }
    }
}
