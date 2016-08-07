using LiteApi.Contracts.Models;
using System;

namespace LiteApi.Contracts.Abstractions
{
    public interface IActionDiscoverer
    {
        ActionContext[] GetActions(ControllerContext controllerCtx);
    }
}
