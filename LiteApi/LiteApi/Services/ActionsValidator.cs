using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using System;
using System.Collections.Generic;

namespace LiteApi.Services
{
    /// <summary>
    /// Class that validates actions before first request is received.
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IActionsValidator" />
    public class ActionsValidator : IActionsValidator
    {
        private readonly IParametersValidator _paramsValidator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionsValidator"/> class.
        /// </summary>
        /// <param name="paramsValidator">The parameters validator.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ActionsValidator(IParametersValidator paramsValidator)
        {
            if (paramsValidator == null) throw new ArgumentNullException(nameof(paramsValidator));
            _paramsValidator = paramsValidator;
        }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <param name="actionCtxs">The action context.</param>
        /// <returns>Collection of strings that contains errors, if not empty an exception should be raised.</returns>
        public IEnumerable<string> GetValidationErrors(ActionContext[] actionCtxs)
        {
            foreach (var action in actionCtxs)
            {
                foreach (var error in _paramsValidator.GetParametersErrors(action))
                {
                    yield return $"Error with parameters in controller '{action.ParentController.Name}' "
                        + $"action '{action.Name}', HTTP method: '{action.HttpMethod}'. Error details: {error}";
                }
            }
        }
    }
}
