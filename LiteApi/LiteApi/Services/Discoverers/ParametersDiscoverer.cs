using System.Reflection;
using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using System.Linq;

namespace LiteApi.Services.Discoverers
{
    /// <summary>
    /// Class for resolving parameters metadata in an action
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IParametersDiscoverer" />
    internal class ParametersDiscoverer : IParametersDiscoverer
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
                bool isFromRoute = param.GetCustomAttribute<FromRouteAttribute>() != null;
                
                ParameterSources source = ParameterSources.Unknown;
                if (isFromQuery && !isFromBody && !isFromRoute) source = ParameterSources.Query;
                else if (!isFromQuery && isFromBody && !isFromRoute) source = ParameterSources.Body;
                else if (!isFromQuery && !isFromBody && isFromRoute) source = ParameterSources.RouteSegment;

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
                    if (parameters[i].IsComplex)
                    {
                        parameters[i].ParameterSource = ParameterSources.Body;
                    }
                    else if (actionCtx.RouteSegments.Any(x => x.IsParameter && x.ParameterName == parameters[i].Name))
                    {
                        parameters[i].ParameterSource = ParameterSources.RouteSegment;
                    }
                    else
                    {
                        parameters[i].ParameterSource = ParameterSources.Query;
                    }
                }
            }

            return parameters;
        }
    }
}
