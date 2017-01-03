using LiteApi.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class InternalLoggerTests
    {
        [Fact]
        public void InternalLogger_NotEnabled_DoesNotThrowExceptionOnNullLogger()
        {
            var logger = new InternalLogger(false, null);
            Assert.True(true); // no exception occurred
        }
        
        [Fact]
        public void InternalLogger_Enabled_ThrowsExceptionOnNullLogger()
        {
            bool error = false;
            try
            {
                var logger = new InternalLogger(true, null);
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void InternalLogger_Enabled_ReturnsTrueForMiddlewareLoggingIsEnabled()
        {
            var mock = new Moq.Mock<ILogger>();
            
            var logger = new InternalLogger(true, mock.Object);
            Assert.True(logger.IsEnabledForMiddleware);
        }

        [Fact]
        public void InternalLogging_Disabled_IsDisabledOnHighestLogLevel()
        {
            var mock = new Moq.Mock<ILogger>();

            var logger = new InternalLogger(false, mock.Object);

            Assert.False(logger.IsEnabled(LogLevel.Critical));
        }

        [Fact]
        public void InternalLogging_CallBeginScope_CallsBase()
        {
            bool calledBase = false;
            var mock = new Moq.Mock<ILogger>();
            mock.Setup(x => x.BeginScope<object>(null)).Callback(() => calledBase = true);
            var logger = new InternalLogger(false, mock.Object);
            Assert.False(calledBase);
            logger.BeginScope<object>(null);
            Assert.True(calledBase);
        }
        
        [Fact]
        public void InternalLogging_EnabledCallLog_CallsBase()
        {
            bool calledBase = false;
            var mock = new Moq.Mock<ILogger>();
            mock.Setup(x => x.Log<object>(LogLevel.Critical, Moq.It.IsAny<EventId>(), null, null, null)).Callback(() => calledBase = true);
            var logger = new InternalLogger(true, mock.Object);
            Assert.False(calledBase);
            logger.Log<object>(LogLevel.Critical, new EventId(), null, null, null);
            Assert.True(calledBase);
        }
        
        [Fact]
        public void InternalLogging_NotEnabledCallLog_DoesNotCallsBase()
        {
            bool calledBase = false;
            var mock = new Moq.Mock<ILogger>();
            mock.Setup(x => x.Log<object>(LogLevel.Critical, Moq.It.IsAny<EventId>(), null, null, null)).Callback(() => calledBase = true);
            var logger = new InternalLogger(false, mock.Object);
            Assert.False(calledBase);
            logger.Log<object>(LogLevel.Critical, new EventId(), null, null, null);
            Assert.False(calledBase);
        }

    }
}
