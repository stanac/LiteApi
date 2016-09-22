using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace LiteApi.Services
{
    /// <summary>
    /// Class that builds controllers.
    /// </summary>
    /// <seealso cref="IControllerBuilder" />
    public class ControllerBuilder : ObjectBuilder, IControllerBuilder
    {
        /// <summary>
        /// Builds the specified controller from controller context and HTTP context.
        /// </summary>
        /// <param name="controllerCtx">The controller context.</param>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>Instance of the built controller.</returns>
        public LiteController Build(ControllerContext controllerCtx, HttpContext httpContext)
        {
            var controller = BuildObject(controllerCtx.ControllerType) as LiteController;
            controller.HttpContext = httpContext;
            return controller;
        }

        
    }
}
