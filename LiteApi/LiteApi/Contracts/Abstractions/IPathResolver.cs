using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Contract for resolving which action (if any) should be invoked for the HTTP request.
    /// </summary>
    public interface IPathResolver
    {
        /// <summary>
        /// Resolves which action (if any) should be invoked for given HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <returns>ActionContext that should be invoked.</returns>
        ActionContext ResolveAction(HttpRequest request);
    }
}
