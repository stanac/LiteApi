using LiteApi.Contracts;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using LiteApi.Attributes;

namespace LiteApi.Services
{
    public class ModelBinder : IModelBinder
    {
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
