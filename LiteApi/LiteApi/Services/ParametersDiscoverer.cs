using System.Reflection;
using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;

namespace LiteApi.Services
{
    /// <summary>
    /// Class for resolving parameters metadata in an action
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IParametersDiscoverer" />
    public class ParametersDiscoverer : IParametersDiscoverer
    {
        /// <summary>
        /// Gets the action parameters metadata for the given action context.
        /// </summary>
        /// <param name="actionCtx">The action context.</param>
        /// <returns>Array of <see cref="ActionParameter"/> retrieved from action context.</returns>
        public ActionParameter[] GetParameters(ActionContext actionCtx)
        {
            var methodParams = actionCtx.Method.GetParameters();
            ActionParameter[] parameters = new ActionParameter[methodParams.Length];
            for (int i = 0; i < methodParams.Length; i++)
            {
                var param = actionCtx.Method.GetParameters()[i];
                bool isFromQuery = param.GetCustomAttribute<FromUrlAttribute>() != null;
                bool isFromBody = param.GetCustomAttribute<FromBodyAttribute>() != null;

                ParameterSources source = ParameterSources.Unknown;
                if (isFromQuery && !isFromBody) source = ParameterSources.Query;
                else if (!isFromQuery && isFromBody) source = ParameterSources.Body;

                parameters[i] = new ActionParameter(actionCtx)
                {
                    Name = param.Name.ToLower(),
                    DefaultValue = param.DefaultValue,
                    HasDefaultValue = param.HasDefaultValue,
                    Type = param.ParameterType,
                    ParameterSource = source
                };

                if (parameters[i].ParameterSource == ParameterSources.Unknown)
                {
                    parameters[i].ParameterSource = parameters[i].IsComplex
                        ? ParameterSources.Body
                        : ParameterSources.Query;
                }
            }

            return parameters;
        }
    }
}
