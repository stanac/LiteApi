using LiteApi.Services.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace LiteApi.Tests
{
    public class ContextAwareLoggerTests
    {
        [Fact]
        public void ContextAwareLogger_NotSetTraceId_TraceIdSetToGuid()
        {
            ILogger logger = new Mock<ILogger>().Object;
            var ctxLogger = new ContextAwareLogger(true, logger, null);
            string traceId = ctxLogger.ContextId.Replace(":", "").Trim();
            Guid tempGuid;
            bool canParse = Guid.TryParse(traceId, out tempGuid);
            Assert.True(canParse);
        }

        [Fact]
        public void ContextAwareLogger_WrittingLogEntry_PrependsTraceId()
        {
            string traceId = Guid.NewGuid().ToString();
            string logEntryText = "log entry";
            var logger = new Fakes.FakeLogger();
            var ctxLog = new ContextAwareLogger(true, logger, traceId);
            ctxLog.LogInformation(logEntryText);
            string logEntry = logger.LogEntries.Single();
            Assert.True(logEntry.StartsWith(traceId + ":"));
            Assert.True(logEntry.EndsWith(logEntryText));
        }
    }
}
