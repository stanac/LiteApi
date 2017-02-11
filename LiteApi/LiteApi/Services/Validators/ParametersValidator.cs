using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LiteApi.Services.Validators
{
    /// <summary>
    /// Class for validating action parameters.
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IParametersValidator" />
    internal class ParametersValidator : IParametersValidator
    {
        /// <summary>
        /// Gets the parameters errors.
        /// </summary>
        /// <param name="actionCtx">The action CTX.</param>
        /// <returns>Collection of strings that contains errors, if not empty an exception should be raised.</returns>
        public IEnumerable<string> GetParametersErrors(ActionContext actionCtx)
        {
            foreach (ActionParameter param in actionCtx.Parameters)
            {
                if (param.ParameterSource == ParameterSources.Unknown)
                {
                    throw new Exception($"Parameter {param.Name} in action {actionCtx.Name} in controller {actionCtx.ParentController?.RouteAndName} doesn't have source set.");
                }
                if (param.ParameterSource == ParameterSources.RouteSegment)
                {
                    if (param.HasDefaultValue)
                    {
                        yield return $"Parameter {param.Name} in action {actionCtx.Name} ({actionCtx.Method} in controller "
                            + $"{actionCtx?.ParentController.RouteAndName} ({actionCtx?.ParentController}) is from route and "
                            + "has default value. Parameters from route cannot have default value.";
                    }
                    if (!param.IsTypeSupportedFromRoute())
                    {
                        yield return $"Parameter {param.Name} in action {actionCtx.Name} ({actionCtx.Method} in controller "
                            + $"{actionCtx?.ParentController.RouteAndName} ({actionCtx?.ParentController}) is from route and "
                            + $"of type {param.Type} which is not supported for route parameter type. Route parameter type are not null "
                            + "types that can be found in ModelBinderCollection.GetSupportedTypesFromUrl()";                    }
                }
            }
            
            if (!actionCtx.HttpMethod.HasBody() && actionCtx.Parameters.Any(x => x.ParameterSource == ParameterSources.Body))
            {
                yield return $"HTTP methods GET and DELETE does not support parameters from body, "
                    + "and there is at least one parameter from body found. " 
                    + $"in action '{actionCtx.Name}' in controller: '{actionCtx.ParentController?.RouteAndName}'. "
                    + AttributeConventions.ErrorResolutionSuggestion;
            }

            if (actionCtx.Parameters.Count(x => x.ParameterSource == ParameterSources.Body) > 1)
            {
                yield return $"Multiple parameters from body found in action '{actionCtx.Name}' in controller '{actionCtx.ParentController?.RouteAndName}'. "
                    + "Maximum number of parameters from body is 1. " + AttributeConventions.ErrorResolutionSuggestion;
            }

            var routeParams = actionCtx.Parameters.Where(x => x.ParameterSource == ParameterSources.RouteSegment).Select(x => x.Name).ToArray();
            var routeSegments = actionCtx.RouteSegments.Where(x => x.IsParameter).Select(x => x.OriginalValue).ToArray();

            foreach (var param in routeParams.Where(x => routeSegments.All(y => $"{{{x}}}" != y)))
            {
                yield return $"Parameter {param} in action {actionCtx.Name} ({actionCtx.Method} in controller "
                        + $"{actionCtx?.ParentController.RouteAndName} ({actionCtx?.ParentController}) is from route and "
                        + $"there is no matching route segment found, use [ActionRoute(\"action/{{{param}}}\")] attribute on action. "
                        + "to add route segment with parameter.";
            }

            foreach (var segment in routeSegments.Where(x => routeParams.All(y => x != $"{{{y}}}")))
            {
                yield return $"Route segment {segment} is set as parameter without matching parameter in method {actionCtx.Method} "
                    + $"in controller {actionCtx.ParentController.RouteAndName} ({actionCtx.ParentController})";
            }


        }
    }
}
