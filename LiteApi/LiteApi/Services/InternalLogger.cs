using Microsoft.Extensions.Logging;
using System;

namespace LiteApi.Services
{
    public class InternalLogger : ILogger
    {
        readonly bool _isEnabled;
        readonly ILogger _logger;

        public InternalLogger(bool isEnabled, ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
            _isEnabled = isEnabled;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return (_logger.BeginScope(state));
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _isEnabled && _logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (_isEnabled)
            {
                _logger.Log(logLevel, eventId, state, exception, formatter);
            }
        }
    }
}
