using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using System.Collections.Generic;
using System.Linq;
using System;

namespace LiteApi.Services
{
    public class ParametersValidator : IParametersValidator
    {
        public IEnumerable<string> GetParametersErrors(ActionContext actionCtx)
        {
            foreach (ActionParameter param in actionCtx.Parameters)
            {
                if (param.ParameterSource == ParameterSources.Unknown)
                {
                    throw new Exception($"Parameter {param.Name} in action {actionCtx.Name} in controller {actionCtx.ParentController?.Name} doesn't have source set.");
                }
            }
            
            if (!actionCtx.HttpMethod.HasBody() && actionCtx.Parameters.Any(x => x.ParameterSource == ParameterSources.Body))
            {
                yield return $"HTTP methods GET and DELETE does not support parameters from body, and there is at least one parameter from body found. " + AttributeConventions.ErrorResolutionSuggestion;
            }

            if (actionCtx.Parameters.Count(x => x.ParameterSource == ParameterSources.Body) > 1)
            {
                yield return "Multiple parameters from body found, maximum number of parameters from body is 1. " + AttributeConventions.ErrorResolutionSuggestion;
            }
        }
    }
}
