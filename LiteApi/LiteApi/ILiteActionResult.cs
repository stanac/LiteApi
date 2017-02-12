using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace LiteApi
{
    /// <summary>
    /// Specialized type of response that does not need to be serialized, but instead can write the response itself
    /// </summary>
    public interface ILiteActionResult
    {
        /// <summary>
        /// Writes the response.
        /// </summary>
        /// <param name="httpCtx">The HTTP context.</param>
        /// <param name="actionCtx">The action context.</param>
        /// <returns>Task to await</returns>
        Task WriteResponse(HttpContext httpCtx, ActionContext actionCtx);
    }
}
