using LiteApi.Contracts.Models;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Contract for action discoverer. Used for discovering actions in given <see cref="ControllerContext"/>
    /// </summary>
    public interface IActionDiscoverer
    {
        /// <summary>
        /// Discovers action in given controller.
        /// </summary>
        /// <param name="controllerCtx">Controller context in which to look for actions.</param>
        /// <returns></returns>
        ActionContext[] GetActions(ControllerContext controllerCtx);
    }
}
