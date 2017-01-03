using System.Collections.Generic;
using System.Security.Claims;
using Xunit;

namespace LiteApi.Tests
{
    public class ClaimExtensionsTests
    {
        [Fact]
        public void ClaimExtensionsNullableInt_NoClaimFound_ReturnsNull()
        {
            List<Claim> list = new List<Claim>();
            int? val = list.GetFirstNullableInt("abc");
            Assert.False(val.HasValue);
        }

        [Fact]
        public void ClaimExtensionsNullableInt_NotParsableClaim_ReturnsNull()
        {
            List<Claim> list = new List<Claim>();
            list.Add(new Claim("abc", "zxc"));
            int? val = list.GetFirstNullableInt("abc");
            Assert.False(val.HasValue);
        }
        
        [Fact]
        public void ClaimExtensionsNullableInt_ParsableClaim_ReturnsValue()
        {
            List<Claim> list = new List<Claim>();
            list.Add(new Claim("abc", " 123 "));
            int? val = list.GetFirstNullableInt("abc");
            Assert.True(val.HasValue);
            Assert.True(val.Value == 123);
        }
        
        [Fact]
        public void ClaimExtensionsString_NoClaimFound_ReturnsNull()
        {
            List<Claim> list = new List<Claim>();
            string val = list.GetFirstAsString("abc");
            Assert.Null(val);
        }

        [Fact]
        public void ClaimExtensionsString_ClaimFound_ReturnsValue()
        {
            List<Claim> list = new List<Claim>();
            list.Add(new Claim("abc", " zxc "));
            string val = list.GetFirstAsString("abc");
            Assert.True(val == " zxc ");
        }
    }
}
