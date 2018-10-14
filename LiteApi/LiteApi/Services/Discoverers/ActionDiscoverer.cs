using LiteApi.Contracts.Models;
using LiteApi.Contracts.Abstractions;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace LiteApi.Services.Discoverers
{
    /// <summary>
    /// Class that is discovering actions in given <see cref="ControllerContext"/>
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IActionDiscoverer" />
    public class ActionDiscoverer : IActionDiscoverer
    {
        private readonly IParametersDiscoverer _parameterDiscoverer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionDiscoverer"/> class.
        /// </summary>
        /// <param name="parameterDiscoverer">The parameter discoverer.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ActionDiscoverer(IParametersDiscoverer parameterDiscoverer)
        {
            _parameterDiscoverer = parameterDiscoverer ?? throw new ArgumentNullException(nameof(parameterDiscoverer));
        }

        /// <summary>
        /// Discovers action in given controller.
        /// </summary>
        /// <param name="controllerCtx">Controller context in which to look for actions.</param>
        /// <returns></returns>
        public virtual ActionContext[] GetActions(ControllerContext controllerCtx)
        {
            var properties = controllerCtx.ControllerType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var propertyMethods = new List<string>();
            propertyMethods.AddRange(properties.Where(x => x.GetMethod?.Name != null).Select(x => x.GetMethod.Name));
            propertyMethods.AddRange(properties.Where(x => x.SetMethod?.Name != null).Select(x => x.SetMethod.Name));

            return controllerCtx.ControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => !propertyMethods.Contains(x.Name))
                .Where(MethodIsAction)
                .Select(x => GetActionContext(x, controllerCtx))
                .ToArray();
        }

        /// <summary>
        /// Checks if methods is action.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        protected virtual bool MethodIsAction(MethodInfo method)
        {
            if (method.GetCustomAttribute<DontMapToApiAttribute>() != null) return false;

            if (method.DeclaringType == typeof(object) || method.DeclaringType == typeof(LiteController)) return false;

            return true;
        }

        /// <summary>
        /// Gets the action context.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="ctrlCtx">The control context.</param>
        /// <returns></returns>
        protected virtual ActionContext GetActionContext(MethodInfo method, ControllerContext ctrlCtx)
        {
            string methodName = method.Name.ToLowerInvariant();
            var segmentsAttr = method.GetCustomAttribute<ActionRouteAttribute>();
            RouteSegment[] segments = new RouteSegment[0];
            if (!ctrlCtx.IsRestful)
            {
                segments = new[] { new RouteSegment(methodName) };
            }
            if (segmentsAttr != null)
            {
                segments = segmentsAttr.RouteSegments.ToArray();
            }

            var actionCtx = new ActionContext
            {
                HttpMethod = GetHttpAttribute(method).Method,
                Method = method,
                ParentController = ctrlCtx,
                RouteSegments = segments
            };
            actionCtx.Parameters = _parameterDiscoverer.GetParameters(actionCtx);
            return actionCtx;
        }

        /// <summary>
        /// Gets the HTTP attribute.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        protected virtual HttpBaseAttribute GetHttpAttribute(MethodInfo method)
        {
            // TODO: replace with inherited attributes
            HttpBaseAttribute attrib = null;
            attrib = method.GetCustomAttribute<HttpGetAttribute>();
            if (attrib != null) return attrib;

            attrib = method.GetCustomAttribute<HttpPostAttribute>();
            if (attrib != null) return attrib;

            attrib = method.GetCustomAttribute<HttpPutAttribute>();
            if (attrib != null) return attrib;

            attrib = method.GetCustomAttribute<HttpDeleteAttribute>();
            if (attrib != null) return attrib;

            return new HttpGetAttribute();
        }
    }
}
