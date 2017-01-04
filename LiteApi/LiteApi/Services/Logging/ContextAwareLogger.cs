using LiteApi.Contracts.Abstractions;
using System;
using Microsoft.Extensions.Logging;

namespace LiteApi.Services.Logging
{
    /// <summary>
    /// Logger that prepends request trace id to the log
    /// </summary>
    /// <seealso cref="LiteApi.Services.InternalLogger" />
    /// <seealso cref="LiteApi.Contracts.Abstractions.IContextAwareLogger" />
    public class ContextAwareLogger : InternalLogger, IContextAwareLogger
    {
        private readonly string _traceId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContextAwareLogger"/> class.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> [is enabled].</param>
        /// <param name="logger">The actual logger.</param>
        /// <param name="traceId">The trace identifier.</param>
        public ContextAwareLogger(bool isEnabled, ILogger logger, string traceId) : base(isEnabled, logger)
        {
            _traceId = (traceId ?? Guid.NewGuid().ToString("N")) + ": ";
        }

        /// <summary>
        /// Gets the request identifier. ContextId is unique for each request
        /// </summary>
        /// <value>
        /// The context identifier.
        /// </value>
        public string ContextId => _traceId;

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Func<TState, Exception, string> ctxFormatter = (inState, ex) => _traceId + formatter(inState, ex);
            base.Log<TState>(logLevel, eventId, state, exception, ctxFormatter);
        }
    }
}
