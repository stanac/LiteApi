using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Builder;

namespace LiteApi.Tests
{
    public class IApplicationBuilderExtensionsTests
    {
        [Fact]
        public void ApplicationBuilderExtension_RegisterMiddleware_CallsRegistraionOfApplicationBuilder()
        {
            var builder = new Fakes.FakeApplicationBuilder();
            ApplicationBuilderExtenstions.UseLiteApi(builder);
            Assert.True(builder.CalledUse);
        }
    }
}
