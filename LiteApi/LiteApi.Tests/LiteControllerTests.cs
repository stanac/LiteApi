using LiteApi.Services;
using LiteApi.Services.Builders;
using System;
using System.Linq;
using Xunit;

namespace LiteApi.Tests
{
    public class LiteControllerTests
    {
        [Fact]
        public void Controller_NotRegisteredUser_ReturnsNullForUser()
        {
            var discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.ComplexRootController));
            var ctrlCtx = discoverer.GetControllers(null).Single();
            var builder = new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object);
            var ctrl = builder.Build(ctrlCtx, new Fakes.FakeHttpContext());
            Assert.Null(ctrl.User);
        }

        [Fact]
        public void Controller_ToStringCall_ReturnsDescriptionOfTheController()
        {
            var ctrl = new Controllers.CollectionController();
            string description = ctrl.ToString();
            Assert.True(description != null && description.StartsWith("CTRL:", StringComparison.Ordinal) && description.ToLower().Contains("collection"));
        }
    }
}
