using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;

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
        /// <returns></returns>
        object[] GetParameterValues(HttpRequest request, ActionContext actionCtx);
    }
}
