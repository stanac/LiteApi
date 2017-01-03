using LiteApi.Contracts.Abstractions;
using System;
using Microsoft.Extensions.Logging;

namespace LiteApi.Services.Logging
{
    internal class ContextAwareLogger : InternalLogger, IContextAwareLogger
    {
        private readonly string _traceId;

        public ContextAwareLogger(bool isEnabled, ILogger logger, string traceId) : base(isEnabled, logger)
        {
            _traceId = (traceId ?? Guid.NewGuid().ToString("N")) + ": ";
        }

        public string ContextId => _traceId;

        public override void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Func<TState, Exception, string> ctxFormatter = (inState, ex) => _traceId + formatter(inState, ex);
            base.Log<TState>(logLevel, eventId, state, exception, ctxFormatter);
        }
    }
}
