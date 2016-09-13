using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
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
        /// <returns></returns>
        Task Invoke(HttpContext httpCtx, ActionContext actionCtx);
    }
}
