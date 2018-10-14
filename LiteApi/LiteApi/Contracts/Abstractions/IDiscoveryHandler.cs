using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Defines method for handling discovery requests (on "/LiteApi/info" path)
    /// </summary>
    public interface IDiscoveryHandler
    {
        /// <summary>
        /// Handles request if needed, returns true if request is handled;
        /// </summary>
        /// <param name="ctx">HTTP context</param>
        /// <returns>True if request is handled</returns>
        Task<bool> HandleIfNeeded(HttpContext ctx);
    }
}
