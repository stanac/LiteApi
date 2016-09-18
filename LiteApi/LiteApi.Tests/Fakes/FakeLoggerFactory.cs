using Microsoft.Extensions.Logging;

namespace LiteApi.Tests.Fakes
{
    public class FakeLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
            // do nothing
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new Logger<FakeLoggerFactory>(new FakeLoggerFactory());
        }

        public void Dispose()
        {
            // do nothing
        }
    }
}
