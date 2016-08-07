using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace LiteApi.Contracts.Abstractions
{
    public interface IControllerBuilder
    {
        LiteController Build(ControllerContext controllerCtx, HttpContext httpContext);
    }
}
