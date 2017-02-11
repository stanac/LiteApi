using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using System;
using System.Linq;
using System.Reflection;

namespace LiteApi.Services.Discoverers
{
    /// <summary>
    /// Instance of this class is used for discovering controllers in given assembly
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IControllerDiscoverer" />
    internal class ControllerDiscoverer : IControllerDiscoverer
    {
        private readonly IActionDiscoverer _actionDiscoverer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerDiscoverer"/> class.
        /// </summary>
        /// <param name="actionDiscoverer">Instance of <see cref="IActionDiscoverer"/></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ControllerDiscoverer(IActionDiscoverer actionDiscoverer)
        {
            if (actionDiscoverer == null) throw new ArgumentNullException(nameof(actionDiscoverer));
            _actionDiscoverer = actionDiscoverer;
        }

        /// <summary>
        /// Discovers controllers
        /// </summary>
        /// <param name="assembly">The assembly in which to look for controllers.</param>
        /// <returns>Array of <see cref="ControllerContext"/>, controllers found in given assembly</returns>
        public ControllerContext[] GetControllers(Assembly assembly)
        {
            var types = assembly.GetTypes()
                .Where(x => typeof(LiteController).IsAssignableFrom(x) && !x.GetTypeInfo().IsAbstract)
                .ToArray();
            ControllerContext[] ctrls = new ControllerContext[types.Length];
            for (int i = 0; i < ctrls.Length; i++)
            {
                //try
                //{
                    ctrls[i] = new ControllerContext
                    {
                        ControllerType = types[i],
                        RouteAndName = GetControllerRute(types[i]),
                        IsRestful = types[i].GetTypeInfo().GetCustomAttributes<RestfulLinksAttribute>().Any()
                    };
                    ctrls[i].Actions = _actionDiscoverer.GetActions(ctrls[i]);
                    ctrls[i].Init();
                //}
                //catch (Exception ex)
                //{
                //    throw;
                //}
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
    
        private static string GetControllerRute(Type ctrlType)
        {
            string root = "api/" + GetControllerName(ctrlType.FullName);
            var rootAttrib = ctrlType.GetTypeInfo().GetCustomAttribute<ControllerRouteAttribute>();
            if (rootAttrib != null)
            {
                root = rootAttrib.Route ?? "";
            }
            return root.ToLower().Trim('/');
        }
    }
}
