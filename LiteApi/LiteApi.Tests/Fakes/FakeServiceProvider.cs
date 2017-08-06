using Moq;
using System;

namespace LiteApi.Tests.Fakes
{
    static class FakeServiceProvider
    {
        public static IServiceProvider GetServiceProvider()
        {
            var mock = new Mock<IServiceProvider>();
            mock.Setup(x => x.GetService(It.IsAny<Type>())).Returns(new Func<Type, object>(
                type =>
                {
                    if (type == typeof(IServiceProvider))
                        return GetServiceProvider();
                    return null;
                }));
            return mock.Object;
        }
    }
}
