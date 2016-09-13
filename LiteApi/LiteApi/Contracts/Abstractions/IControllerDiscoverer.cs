using LiteApi.Contracts.Models;
using System.Reflection;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Contract of controller discoverer, used to discover controllers in given assembly.
    /// </summary>
    public interface IControllerDiscoverer
    {
        /// <summary>
        /// Discovers controllers in given assembly
        /// </summary>
        /// <param name="assembly">The assembly in which to look for controllers.</param>
        /// <returns>Array of <see cref="ControllerContext"/>, discovered controllers.</returns>
        ControllerContext[] GetControllers(Assembly assembly);
    }
}
