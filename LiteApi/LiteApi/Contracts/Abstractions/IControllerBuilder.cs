using LiteApi.Contracts.Models;

namespace LiteApi.Contracts.Abstractions
{
    public interface IControllerBuilder
    {
        LiteController Build(ControllerContext controllerCtx);
    }
}
