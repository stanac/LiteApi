using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class LiteApiRegistrationExceptionTests
    {
        [Fact]
        public void LiteApiRegistrationException_NotSetErrors_ErrorsAreNotNull()
        {
            var ex1 = new LiteApiRegistrationException("123");
            var ex2 = new LiteApiRegistrationException("123", new Exception());
            Assert.NotNull(ex1.Errors);
            Assert.NotNull(ex2.Errors);
        }
        
        [Fact]
        public void LiteApiRegistrationException_SetErrors_SetsErrors()
        {
            var ex = new LiteApiRegistrationException("message", new[] { "error" });
            Assert.True(ex.Errors.Any());
        }
    }
}
