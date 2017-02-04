using System;
using System.Collections.Concurrent;
using Xunit;

namespace LiteApi.Tests
{
    public class ExtensionTests
    {
        [Fact]
        public void TypeFriendlyNameTests()
        {
            Type t = typeof(ConcurrentDictionary<int?, ExtensionTests>[]);
            var fullName = t.GetFriendlyName(TypeFullName.FullName);
            var shortName = t.GetFriendlyName(TypeFullName.ShortName);
            var automaticName = t.GetFriendlyName(TypeFullName.FullNameForUncommonTypes);

            Assert.Equal("System.Collections.Concurrent.ConcurrentDictionary<System.Int32?, LiteApi.Tests.ExtensionTests>[]", fullName);
            Assert.Equal("ConcurrentDictionary<Int32?, ExtensionTests>[]", shortName);
            Assert.Equal("System.Collections.Concurrent.ConcurrentDictionary<Int32?, LiteApi.Tests.ExtensionTests>[]", automaticName);
        }
    }
}
