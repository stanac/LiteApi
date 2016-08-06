using LiteApi.Attributes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace LiteApi.Contracts
{
    public class ActionContext
    {
        public string Name { get; set; }
        public ActionParameter[] Parameters { get; set; }
        public string HttpMethod { get; set; }
        public MethodInfo Method { get; set; }
        public ControllerContext ParentController { get; set; }

        internal static ActionContext ResolveFromMethod(MethodInfo info, string parentController)
        {
            List<ActionParameter> parameters = new List<ActionParameter>();
            foreach (ParameterInfo param in info.GetParameters())
            {
                var isQuery = param.GetCustomAttribute<FromUrlAttribute>() != null;
                if (!isQuery && param.GetCustomAttribute<FromBodyAttribute>() == null)
                {
                    throw new InvalidOperationException($"Parameter {param.Name} of action {info.Name} of controller {parentController} needs to be marked with [FromUrl] or with [FromBody] attribute.");
                }
                parameters.Add(new ActionParameter
                {
                    Name = param.Name.ToLower(),
                    DefaultValue = param.DefaultValue,
                    HasDefaultValue = param.HasDefaultValue,
                    Type = param.ParameterType,
                    IsQuery = isQuery
                });
            }
            return new ActionContext
            {
                Name = info.Name.ToLower(),
                Parameters = parameters.ToArray(),
                Method = info
            };
        }

        internal object[] ParseParameters(HttpRequest request)
        {
            List<object> args = new List<object>();
            foreach (var param in Parameters)
            {
                string valueString;
                if (param.IsQuery)
                {
                    valueString = request.Query[param.Name];
                    if (valueString == null)
                    {
                        if (param.HasDefaultValue)
                        {
                            args.Add(param.DefaultValue);
                            continue;
                        }
                        else
                        {
                            throw new InvalidOperationException($"Parameter {param.Name} is missing and has no default value.");
                        }
                    }
                }
                else
                {
                    using (TextReader reader = new StreamReader(request.Body))
                    {
                        valueString = reader.ReadToEnd();
                    }
                    request.Body.Dispose();
                }
                args.Add(param.ParseValue(valueString));
            }
            return args.ToArray();
        }
    }
}
