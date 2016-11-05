using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Filter that can be used to deny access to controllers and actions
    /// </summary>
    public interface IApiFilter
    {
        /// <summary>
        /// When implemented can be called to check if controller/action should be invoked or not (e.g. for authorization/authentication)
        /// </summary>
        /// <param name="httpCtx">HTTP context</param>
        /// <returns>Should continue and if not which status code and message to set.</returns>
        ApiFilterRunResult ShouldContinue(HttpContext httpCtx);
    }

    /// <summary>
    /// Asynchronous variant of <see cref="IApiFilter"/>
    /// </summary>
    public interface IApiFilterAsync
    {
        /// <summary>
        /// When implemented can be called to check if controller/action should be invoked or not (e.g. for authorization/authentication) in async fashion
        /// </summary>
        /// <param name="httpCtx">HTTP context</param>
        /// <returns>Should continue and if not which status code and message to set.</returns>
        Task<ApiFilterRunResult> ShouldContinueAsync(HttpContext httpCtx);
    }

    /// <summary>
    /// Result of call to API filter
    /// </summary>
    public class ApiFilterRunResult
    {
        /// <summary>
        /// Value that tells if call to controller/action should be invoked
        /// </summary>
        public bool ShouldContinue { get; set; }

        /// <summary>
        /// If set and ShouldContinue is false, this value will be set to response
        /// </summary>
        public int? SetResponseCode { get; set; }

        /// <summary>
        /// Gets or sets the set response message.
        /// </summary>
        /// <value>
        /// The set response message.
        /// </value>
        public string SetResponseMessage { get; set; }
    }
}
