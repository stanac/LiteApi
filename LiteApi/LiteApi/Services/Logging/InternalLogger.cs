using Microsoft.Extensions.Logging;
using System;

namespace LiteApi.Services
{
    /// <summary>
    /// Internal logger
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Logging.ILogger" />
    public class InternalLogger : ILogger
    {
        readonly bool _isEnabled;
        readonly ILogger _logger;

        /// <summary>
        /// Gets a value indicating whether logging is enabled at middleware level.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance logging is enabled at middleware level; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabledForMiddleware { get { return _isEnabled; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalLogger"/> class.
        /// </summary>
        /// <param name="isEnabled">if set to <c>true</c> logging is enabled.</param>
        /// <param name="logger">The actual logger.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public InternalLogger(bool isEnabled, ILogger logger)
        {
            if (isEnabled)
            {
                if (logger == null) throw new ArgumentNullException(nameof(logger));
            }
            _logger = logger;
            _isEnabled = isEnabled;
        }

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>
        /// An IDisposable that ends the logical operation scope on dispose.
        /// </returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            return (_logger.BeginScope(state));
        }

        /// <summary>
        /// Checks if the given <paramref name="logLevel" /> and logging setting is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns>
        ///   <c>true</c> if enabled.
        /// </returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return _isEnabled && _logger.IsEnabled(logLevel);
        }

        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <c>string</c> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public virtual void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (_isEnabled)
            {
                _logger.Log(logLevel, eventId, state, exception, formatter);
            }
        }
    }
}
