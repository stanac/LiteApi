using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using System;
using System.Linq;
using System.Reflection;

namespace LiteApi.Services
{
    public class ControllerDiscoverer : IControllerDiscoverer
    {
        private readonly IActionDiscoverer _actionDiscoverer;

        public ControllerDiscoverer(IActionDiscoverer actionDiscoverer)
        {
            if (_actionDiscoverer == null) throw new ArgumentNullException(nameof(actionDiscoverer));
            _actionDiscoverer = actionDiscoverer;
        }

        public ControllerContext[] GetControllers(Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(x => typeof(LiteController).IsAssignableFrom(x) && !x.GetTypeInfo().IsAbstract)
                .ToArray();
            ControllerContext[] ctrls = new ControllerContext[types.Length];
            for (int i = 0; i < ctrls.Length; i++)
            {
                ctrls[i] = new ControllerContext
                {
                    ControllerType = types[i],
                    Name = GetControllerName(types[i].FullName),
                    UrlRoot = GetControllerRoot(types[i])
                };
                ctrls[i].Actions = _actionDiscoverer.GetActions(ctrls[i]);
            }

            return ctrls;
        }

        private static string GetControllerName(string typeFullName)
        {
            const string controller = "controller";
            string name = typeFullName.Split('.').Last().ToLowerInvariant();
            if (name.EndsWith(controller, StringComparison.Ordinal))
            {
                name = name.Substring(0, name.Length - controller.Length);
            }
            return name;
        }

        private static string GetControllerRoot(Type ctrlType)
        {
            string root = "api";
            var rootAttrib = ctrlType.GetTypeInfo().GetCustomAttribute<UrlRootAttribute>();
            if (rootAttrib != null)
            {
                root = rootAttrib.UrlRoot ?? "";
            }
            return root;
        }
    }
}
