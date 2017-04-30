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
        /// Gets a value indicating whether <see cref="LiteApi.Attributes.SkipFiltersAttribute "/> should be ignored.
        /// </summary>
        /// <value>
        ///   <c>true</c> if SkipFiltersAttribute should be ignored; otherwise, <c>false</c>.
        /// </value>
        bool IgnoreSkipFilters { get; }

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
        /// Gets a value indicating whether <see cref="LiteApi.Attributes.SkipFiltersAttribute "/> should be ignored.
        /// </summary>
        /// <value>
        ///   <c>true</c> if SkipFiltersAttribute should be ignored; otherwise, <c>false</c>.
        /// </value>
        bool IgnoreSkipFilters { get; }

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

        /// <summary>
        /// Gets the unauthorized result.
        /// </summary>
        /// <value>
        /// The unauthorized result.
        /// </value>
        public static ApiFilterRunResult Unauthorized
            => new ApiFilterRunResult
            {
                ShouldContinue = false,
                SetResponseCode = 403,
                SetResponseMessage = "User is unauthorized to access the resource"
            };

        /// <summary>
        /// Gets the continue true result.
        /// </summary>
        /// <value>
        /// The continue true result.
        /// </value>
        public static ApiFilterRunResult Continue
            => new ApiFilterRunResult
            {
                ShouldContinue = true
            };

        /// <summary>
        /// Gets the unauthenticated result.
        /// </summary>
        /// <value>
        /// The unauthenticated result.
        /// </value>
        public static ApiFilterRunResult Unauthenticated
            => new ApiFilterRunResult
            {
                ShouldContinue = false,
                SetResponseCode = 401,
                SetResponseMessage = "User is unauthenticated"
            };
    }
}
