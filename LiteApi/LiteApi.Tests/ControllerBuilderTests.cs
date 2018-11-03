using LiteApi.Services;
using LiteApi.Services.Builders;
using System;
using Xunit;

namespace LiteApi.Tests
{
    public class ControllerBuilderTests
    {
        [Fact]
        public void ControllerBuilder_ControllerWithMultipleConstructors_WillUseOneMarkedWithApiContructorAttr()
        {
            var ctrlBuilder = new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object);
            var ctrl = ctrlBuilder.BuildObject<Controllers.BuilderTestController>();
            Assert.Equal("default", ctrl.StrVal);
        }
    }
}
