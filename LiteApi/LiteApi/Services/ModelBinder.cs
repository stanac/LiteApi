using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;

namespace LiteApi.Services
{
    /// <summary>
    /// Class for resolving parameter values for given <see cref="ActionContext"/>
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IModelBinder" />
    public class ModelBinder : IModelBinder
    {
        /// <summary>
        /// Gets the parameter values from the HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="actionCtx">The action context which should be invoked.</param>
        /// <returns></returns>
        public object[] GetParameterValues(HttpRequest request, ActionContext actionCtx)
        {
            object[] values = new object[actionCtx.Parameters.Length];
            List<object> args = new List<object>();
            foreach (var param in actionCtx.Parameters)
            {
                string valueString = null;
                if (param.ParameterSource == ParameterSources.Query)
                {
                    valueString = request.Query[param.Name];
                    if (valueString == null)
                    {
                        if (param.HasDefaultValue)
                        {
                            args.Add(param.DefaultValue);
                            continue;
                        }
                        if (param.ParameterSource == ParameterSources.Query)
                        {
                            throw new InvalidOperationException($"Parameter {param.Name} is missing and has no default value.");
                        }
                    }
                }
                else if (param.ParameterSource == ParameterSources.Body)
                {
                    using (TextReader reader = new StreamReader(request.Body))
                    {
                        valueString = reader.ReadToEnd();
                    }
                    request.Body.Dispose();
                }
                else
                {
                    throw new ArgumentException(
                        $"Parameter {param.Name} in controller {actionCtx.ParentController.Name} in action {actionCtx.Name} "
                        + "has unknown source (body or URL). " + AttributeConventions.ErrorResolutionSuggestion);
                }

                args.Add(param.ParseValue(valueString));
            }
            return args.ToArray();
        }
    }
}
