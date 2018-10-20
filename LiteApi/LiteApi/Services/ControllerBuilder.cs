using System;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace LiteApi.Services
{
    /// <summary>
    /// Class that builds controllers.
    /// </summary>
    /// <seealso cref="IControllerBuilder" />
    public class ControllerBuilder : ObjectBuilderIL, IControllerBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerBuilder"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public ControllerBuilder(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// Builds the specified controller from controller context and HTTP context.
        /// </summary>
        /// <param name="controllerCtx">The controller context.</param>
        /// <param name="httpContext">The HTTP context.</param>
        /// <returns>Instance of the built controller.</returns>
        public virtual LiteController Build(ControllerContext controllerCtx, HttpContext httpContext)
        {
            var controller = BuildObject(controllerCtx.ControllerType) as LiteController;
            controller.HttpContext = httpContext;
            return controller;
        }

        
    }
}
