using System.Reflection;
using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using System.Linq;
using System;

namespace LiteApi.Services.Discoverers
{
    /// <summary>
    /// Class for resolving parameters metadata in an action
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IParametersDiscoverer" />
    public class ParametersDiscoverer : IParametersDiscoverer
    {
        private readonly IServiceProvider _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParametersDiscoverer"/> class.
        /// </summary>
        /// <param name="services">The service provider.</param>
        public ParametersDiscoverer(IServiceProvider services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            _services = services;
        }

        /// <summary>
        /// Gets the action parameters metadata for the given action context.
        /// </summary>
        /// <param name="actionCtx">The action context.</param>
        /// <returns>Array of <see cref="ActionParameter"/> retrieved from action context.</returns>
        public virtual ActionParameter[] GetParameters(ActionContext actionCtx)
        {
            var methodParams = actionCtx.Method.GetParameters();
            ActionParameter[] parameters = new ActionParameter[methodParams.Length];
            for (int i = 0; i < methodParams.Length; i++)
            {
                var param = actionCtx.Method.GetParameters()[i];
                bool isFromQuery = false;
                bool isFromBody = false;
                bool isFromRoute = false;
                bool isFromService = false;
                bool isFromHeader = false;
                string overridenName = null;

                // todo: refactor with attribute inheritance
                if (param.GetCustomAttribute<FromServicesAttribute>() != null)
                {
                    isFromService = true;
                }
                else
                {
                    isFromQuery = param.GetCustomAttribute<FromQueryAttribute>() != null;
                    isFromBody = param.GetCustomAttribute<FromBodyAttribute>() != null;
                    isFromRoute = param.GetCustomAttribute<FromRouteAttribute>() != null;
                    var headerAttrib = param.GetCustomAttribute<FromHeaderAttribute>();
                    if (headerAttrib != null)
                    {
                        isFromHeader = true;
                        overridenName = headerAttrib.HeaderName;
                    }
                }

                ParameterSources source = ParameterSources.Unknown;

                if (isFromService) source = ParameterSources.Service;
                else if (isFromHeader) source = ParameterSources.Header;
                else if (isFromQuery && !isFromBody && !isFromRoute) source = ParameterSources.Query;
                else if (!isFromQuery && isFromBody && !isFromRoute) source = ParameterSources.Body;
                else if (!isFromQuery && !isFromBody && isFromRoute) source = ParameterSources.RouteSegment;

                parameters[i] = new ActionParameter(actionCtx, new ModelBinders.ModelBinderCollection(new JsonSerializer(), _services))
                {
                    Name = param.Name.ToLower(),
                    DefaultValue = param.DefaultValue,
                    HasDefaultValue = param.HasDefaultValue,
                    Type = param.ParameterType,
                    ParameterSource = source,
                    OverridenName = overridenName
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
