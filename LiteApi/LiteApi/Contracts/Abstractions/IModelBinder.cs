using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Contract for resolving parameter values for given <see cref="ActionContext"/>
    /// </summary>
    public interface IModelBinder
    {
        /// <summary>
        /// Gets the parameter values from the HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="actionCtx">The action context which should be invoked.</param>
        /// <returns>Collection of parameters to be passed to action when being invoked</returns>
        object[] GetParameterValues(HttpRequest request, ActionContext actionCtx);

        /// <summary>
        /// Checks if type is supported by model binder instance.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> is parameter is supported, otherwise <c>false</c></returns>
        bool DoesSupportType(Type type, ParameterSources source);
        
    }
}
