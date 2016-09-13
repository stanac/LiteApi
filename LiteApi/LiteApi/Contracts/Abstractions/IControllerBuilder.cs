using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// When implemented builds controllers.
    /// </summary>
    public interface IControllerBuilder
    {
        /// <summary>
        /// Builds the specified controller from controller context and HTTP context.
        /// </summary>
        /// <param name="controllerCtx">The controller context.</param>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>Instance of the built controller.</returns>
        LiteController Build(ControllerContext controllerCtx, HttpContext httpContext);
    }
}
