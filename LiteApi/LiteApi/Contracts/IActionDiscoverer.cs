using System;

namespace LiteApi.Contracts
{
    public interface IActionDiscoverer
    {
        ActionContext[] GetActions(ControllerContext controllerCtx);
    }
}
