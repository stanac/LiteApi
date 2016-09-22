using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Custom API filter that can be set in <see cref="Attributes.AuthorizeFilterAttribute"/>
    /// <example>
    /// // NOTE: MyFilter class implements ICustomApiFilter
    /// [AuthorizeFilter(typeof(MyFilter))]
    /// </example>
    /// </summary>
    public interface ICustomApiFilter
    {
        /// <summary>
        /// Gets a value indicating whether ShouldContinueAsync should be run.
        /// </summary>
        /// <value>
        /// <c>true</c> if ShouldContinueAsync should be executed; otherwise, <c>false</c> in which case ShouldContinue will be executed.
        /// </value>
        bool IsAsync { get; }

        /// <summary>
        /// Gets the synchronous should continue. Must not be null if <see cref="IsAsync"/> is false;
        /// </summary>
        /// <value>
        /// The should continue check.
        /// </value>
        Func<HttpContext, bool> ShouldContinue { get; }

        /// <summary>
        /// Asynchronous variant of <see cref="ShouldContinue"/> executed if <see cref="IsAsync"/> is true.
        /// </summary>
        /// <value>
        /// The asynchronous should continue check.
        /// </value>
        Func<HttpContext, Task<bool>> ShouldContinueAsync { get; }
    }
}
