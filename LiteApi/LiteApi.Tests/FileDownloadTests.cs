using LiteApi.Services;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class FileDownloadTests
    {
        [Fact]
        public async Task CanDownloadFileFromBytes()
        {
            await AssertDownloadAction("download1");
        }

        [Fact]
        public async Task CanDownloadFileFromStream()
        {
            await AssertDownloadAction("download2");
        }

        [Fact]
        public void FileDownloadActionResult_NullParameter_ThrowsException()
        {
            Action<byte[], string, string, bool> assertException = (data, contentType, name, expectesException) =>
            {
                bool error = false;
                try
                {
                    var a = new FileDownloadActionResult(data, contentType, name);
                }
                catch (ArgumentNullException)
                {
                    error = true;
                }
                Assert.True(error == expectesException);
            };
            Action<Stream, string, string, bool> assertException2 = (data, contentType, name, expectesException) =>
            {
                bool error = false;
                try
                {
                    var a = new FileDownloadActionResult(data, contentType, name);
                }
                catch (ArgumentNullException)
                {
                    error = true;
                }
                Assert.True(error == expectesException);
            };

            assertException(null, "a", "b", true);
            assertException(new byte[0], null, "b", true);
            assertException(new byte[0], "a", null, true);
            assertException(new byte[0], "a", "b", false);
            assertException2(null, "a", "b", true);
            assertException2(new MemoryStream(), null, "b", true);
            assertException2(new MemoryStream(), "a", null, true);
            assertException2(new MemoryStream(), "a", "b", false);

        }

        private async Task AssertDownloadAction(string actionName)
        {
            var httpCtx = new Fakes.FakeHttpContext();
            var discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.FileDownloadController));
            var ctrlCtx = discoverer.GetControllers(null).Single();
            var builder = new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object);
            var invoker = new ActionInvoker(builder, new Services.ModelBinders.ModelBinderCollection(new JsonSerializer(), new Moq.Mock<IServiceProvider>().Object));
            var action = ctrlCtx.Actions.First(x => x.Name == actionName);
            await invoker.Invoke(httpCtx, action);
            string response = httpCtx.Response.ReadBody();
            Assert.Equal(actionName, response);
        }
    }
}
