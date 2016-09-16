using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Contract for model binder where source of the model is query
    /// </summary>
    public interface IQueryModelBinder
    {
        /// <summary>
        /// Gets or sets the supported types.
        /// </summary>
        /// <value>
        /// The supported types.
        /// </value>
        IEnumerable<Type> SupportedTypes { get; }

        /// <summary>
        /// Checks if type is supported by query model binder implementation.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        ///   <c>True</c> if type is supported, otherwise 
        /// </returns>
        bool DoesSupportType(Type type);

        /// <summary>
        /// Parses query parameter
        /// </summary>
        /// <param name="request">HTTP request from which to retrieve value(s).</param>
        /// <param name="actionCtx">Action context of the parameter.</param>
        /// <param name="parameter">The parameter to parse.</param>
        /// <returns>object, Value of the parameter</returns>
        object ParseParameterValue(HttpRequest request, ActionContext actionCtx, ActionParameter parameter);
    }
}
