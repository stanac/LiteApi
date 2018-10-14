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
    public class ControllerDiscoverer : IControllerDiscoverer
    {
        private readonly IActionDiscoverer _actionDiscoverer;
        private readonly ILiteApiOptionsAccessor _optionsRetriever;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerDiscoverer"/> class.
        /// </summary>
        /// <param name="actionDiscoverer">Instance of <see cref="IActionDiscoverer"/></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ControllerDiscoverer(IActionDiscoverer actionDiscoverer, ILiteApiOptionsAccessor optionsRetriever)
        {
            _actionDiscoverer = actionDiscoverer ?? throw new ArgumentNullException(nameof(actionDiscoverer));
            _optionsRetriever = optionsRetriever ?? throw new ArgumentNullException(nameof(_optionsRetriever));
        }

        /// <summary>
        /// Discovers controllers
        /// </summary>
        /// <param name="assembly">The assembly in which to look for controllers.</param>
        /// <returns>Array of <see cref="ControllerContext"/>, controllers found in given assembly</returns>
        public virtual ControllerContext[] GetControllers(Assembly assembly)
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
                    RouteAndName = GetControllerRoute(types[i]),
                    IsRestful = types[i].GetTypeInfo().GetCustomAttributes<RestfulAttribute>().Any()
                };
                ctrls[i].Actions = _actionDiscoverer.GetActions(ctrls[i]);
                ctrls[i].Init(_optionsRetriever);
            }

            return ctrls;
        }

        /// <summary>
        /// Gets the name of the controller.
        /// </summary>
        /// <param name="typeFullName">Full name of the type.</param>
        /// <returns></returns>
        protected virtual string GetControllerName(string typeFullName)
        {
            const string controller = "controller";
            string name = typeFullName.Split('.').Last().ToLowerInvariant();
            if (name.EndsWith(controller, StringComparison.Ordinal))
            {
                name = name.Substring(0, name.Length - controller.Length);
            }
            return name;
        }

        /// <summary>
        /// Gets the controller route.
        /// </summary>
        /// <param name="ctrlType">Type of the control.</param>
        /// <returns></returns>
        protected virtual string GetControllerRoute(Type ctrlType)
        {
            string urlRoot = _optionsRetriever.GetOptions().UrlRoot;
            string route = urlRoot + GetControllerName(ctrlType.FullName);
            var rootAttrib = ctrlType.GetTypeInfo().GetCustomAttribute<ControllerRouteAttribute>();
            if (rootAttrib != null)
            {
                route = rootAttrib.Route ?? "";
            }
            return route.ToLower().Trim('/');
        }
    }
}
