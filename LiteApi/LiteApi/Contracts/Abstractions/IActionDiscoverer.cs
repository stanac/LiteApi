using LiteApi.Contracts.Models;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Contract for action discoverer. Used for discovering actions in given <see cref="ControllerContext"/>
    /// </summary>
    public interface IActionDiscoverer
    {
        ActionContext[] GetActions(ControllerContext controllerCtx);
    }
}
