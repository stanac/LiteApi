using LiteApi.Services;
using Xunit;

namespace LiteApi.Tests
{
    public class ControllerBuilderTests
    {
        [Fact]
        public void ControllerBuilder_ControllerWithMultipleConstructors_WillUseOneMarkedWithApiContructorAttr()
        {
            var ctrlBuilder = new ControllerBuilder();
            var ctrl = ctrlBuilder.BuildObject<Controllers.BuilderTestController>();
            Assert.Equal("default", ctrl.StrVal);
        }
    }
}
