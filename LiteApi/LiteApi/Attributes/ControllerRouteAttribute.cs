using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi
{
    /// <summary>
    /// Can be used to change controller name or route when matching URL.
    /// </summary>
    /// <example>[ControllerRoute("/api/v2/person")]</example>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerRouteAttribute : Attribute
    {
        /// <summary>
        /// Gets the route.
        /// </summary>
        /// <value>
        /// The route.
        /// </value>
        public string Route { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerRouteAttribute"/> class.
        /// </summary>
        /// <example>[ControllerRoute("/api/v2/person")]</example>
        /// <param name="name">The name and route of the controller to use when matching URL.</param>
        public ControllerRouteAttribute(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Route = name.Trim().Replace("\\", "/").TrimStart('/').TrimEnd('/');
        }
    }
}
