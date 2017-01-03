using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

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
        /// <param name="logger">Logger to use for logging</param>
        /// <returns>ActionContext that should be invoked.</returns>
        ActionContext ResolveAction(HttpRequest request, ILogger logger);
    }
}
