using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class ControllerRouteAttributeTests
    {
        [Fact]
        public void ControllerRouteAttribute_BackslashesAreReplaced()
        {
            var attrib = new ControllerRouteAttribute("a/b\\c\\d");
            int backslashCount = attrib.Route.Count(x => x == '\\');
            Assert.Equal(0, backslashCount);
        }
        
        [Fact]
        public void ControllerRouteAttribute_SlashesAreTrimmed()
        {
            var attrib = new ControllerRouteAttribute("/a/b/c/d/");
            
            Assert.False(attrib.Route.StartsWith("/", StringComparison.Ordinal));
            Assert.False(attrib.Route.EndsWith("/", StringComparison.Ordinal));
        }
        
        [Fact]
        public void ControllerRouteAttribute_NullName_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = new ControllerRouteAttribute(null);
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);
        }
    }
}
