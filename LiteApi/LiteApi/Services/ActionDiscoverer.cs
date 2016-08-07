using LiteApi.Attributes;
using LiteApi.Contracts;
using System;
using System.Linq;
using System.Reflection;

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
            => controllerCtx.ControllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(MethodIsAction)
                .Select(x => GetActionContext(x, controllerCtx))
                .ToArray();

        private static bool MethodIsAction(MethodInfo method)
            => GetHttpAttribute(method) != null;

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
