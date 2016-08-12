using LiteApi.Attributes;
using LiteApi.Contracts.Models;
using LiteApi.Contracts.Abstractions;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace LiteApi.Services
{
    public class ActionDiscoverer : IActionDiscoverer
    {
        private readonly IParametersDiscoverer _parameterDiscoverer;

        public ActionDiscoverer(IParametersDiscoverer parameterDiscoverer)
        {
            if (parameterDiscoverer == null) throw new ArgumentNullException(nameof(parameterDiscoverer));
            _parameterDiscoverer = parameterDiscoverer;
        }

        public ActionContext[] GetActions(ControllerContext controllerCtx)
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

        private static bool MethodIsAction(MethodInfo method)
        {
            if (method.GetCustomAttribute<DontMapToApiAttribute>() != null) return false;

            if (method.DeclaringType == typeof(object)) return false;

            return true;
        }

        private ActionContext GetActionContext(MethodInfo method, ControllerContext ctrlCtx)
        {
            var actionCtx = new ActionContext
            {
                HttpMethod = GetHttpAttribute(method).Method,
                Method = method,
                Name = method.Name.ToLowerInvariant(),
                ParentController = ctrlCtx
            };
            actionCtx.Parameters = _parameterDiscoverer.GetParameters(actionCtx);
            return actionCtx;
        }

        private static HttpBaseAttribute GetHttpAttribute(MethodInfo method)
        {
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
