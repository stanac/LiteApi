using Microsoft.Extensions.Logging;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Contract for logger that is aware of HTTP context
    /// </summary>
    internal interface IContextAwareLogger: ILogger
    {
        /// <summary>
        /// Gets the context identifier. ContextId is unique for each request
        /// </summary>
        /// <value>
        /// The context identifier.
        /// </value>
        string ContextId { get; }
    }
}
