using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi.Services.Validators
{
    /// <summary>
    /// Class that validates actions before first request is received.
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IActionsValidator" />
    internal class ActionsValidator : IActionsValidator
    {
        private readonly IParametersValidator _paramsValidator;
        private readonly IAuthorizationPolicyStore _policyStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionsValidator"/> class.
        /// </summary>
        /// <param name="paramsValidator">The parameters validator.</param>
        /// <param name="policyStore">Authorization policy store.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ActionsValidator(IParametersValidator paramsValidator, IAuthorizationPolicyStore policyStore)
        {
            if (policyStore == null) throw new ArgumentNullException(nameof(policyStore));
            if (paramsValidator == null) throw new ArgumentNullException(nameof(paramsValidator));
            _paramsValidator = paramsValidator;
            _policyStore = policyStore;
        }

        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <param name="actionCtxs">The action context.</param>
        /// <param name="isControllerRestful">True if parent controller is restful (has RestfulLinksAttribute)</param>
        /// <returns>Collection of strings that contains errors, if not empty an exception should be raised.</returns>
        public IEnumerable<string> GetValidationErrors(ActionContext[] actionCtxs, bool isControllerRestful)
        {
            foreach (var action in actionCtxs)
            {
                foreach (var error in ActionSegmentsValidator.GetRouteSegmentsErrors(action, isControllerRestful))
                {
                    yield return error;
                }
                foreach (var missingPolicy in GetMissingAuthorizationPolicies(action))
                {
                    yield return $"Action '{action.Name}'({action.Method}), HTTP method: '{action.HttpMethod}' in controller "
                        + $"{action.ParentController.RouteAndName} has defined authorization policy {missingPolicy} which is not "
                        + "registered within middleware. "
                        + "Use LiteApiOptions.AddAuthorizationPolicy to register authorization policy when registering middleware.";
                }
                foreach (var error in _paramsValidator.GetParametersErrors(action))
                {
                    yield return $"Error with parameters in controller '{action.ParentController.RouteAndName}'({action.ParentController.ControllerType}) "
                        + $"action '{action.Name}'({action.Method}), HTTP method: '{action.HttpMethod}'. Error details: {error}";
                }
            }
        }


        private IEnumerable<string> GetMissingAuthorizationPolicies(ActionContext actionCtx)
        {
            return actionCtx
                .Method
                .GetAttributesAs<IPolicyApiFilter>()
                .Select(x => x.PolicyName)
                .Where(x => _policyStore.GetPolicy(x) == null);
        }
    }
}
