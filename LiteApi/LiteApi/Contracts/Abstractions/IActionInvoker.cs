using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// When implemented it's used for invoking actions
    /// </summary>
    public interface IActionInvoker
    {
        /// <summary>
        /// Invokes the specified <see cref="ActionContext"/>.
        /// </summary>
        /// <param name="httpCtx">The HTTP context, set by the middleware.</param>
        /// <param name="actionCtx">The action CTX.</param>
        /// <param name="logger">Logger to use</param>
        /// <returns></returns>
        Task Invoke(HttpContext httpCtx, ActionContext actionCtx, ILogger logger);
    }
}
