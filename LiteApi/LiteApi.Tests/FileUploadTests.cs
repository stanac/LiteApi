using LiteApi.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class FileUploadTests
    {
        [Fact]
        public async Task CanInvokeFileUpload()
        {
            var httpCtx = GetHttpCtx();

            var discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.FileUploadController));
            var ctrlCtx = discoverer.GetControllers(null).Single();
            var builder = new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object);
            var invoker = new ActionInvoker(builder, new Services.ModelBinders.ModelBinderCollection(new JsonSerializer(), new Moq.Mock<IServiceProvider>().Object), new JsonSerializer());
            await invoker.Invoke(httpCtx, ctrlCtx.Actions.Single());
            string response = httpCtx.Response.ReadBody();
            Assert.Equal("2", response);
        }

        [Fact]
        public void FormFileCollection_NullRequest_ThrowsException()
        {
            bool error = false;
            try
            {
                var a = CreateFormFileCollection(null);
            }
            catch (TargetInvocationException ex)
            {
                error = ex.InnerException is ArgumentNullException;
            }
            Assert.True(error);
        }

        [Fact]
        public void ForFileCollection_NonEmpty_CanReturnFiles()
        {
            var collection = CreateFormFileCollection(GetHttpCtx().Request);

            Assert.Equal(2, collection.FileCount);
            Assert.True(collection.HasFiles);
            Assert.Equal(2, collection.Files.Count);
        }

        [Fact]
        public void ForFileCollection_Empty_CannotReturnFiles()
        {
            var collection = CreateFormFileCollection(GetHttpCtx(false).Request);

            Assert.Equal(0, collection.FileCount);
            Assert.False(collection.HasFiles);
            Assert.Equal(0, collection.Files.Count);

            AssertIndexOutOfRange(() => { var a = collection.Files[0]; });
            AssertIndexOutOfRange(() => { var a = collection.Files["a"]; });
            AssertIndexOutOfRange(() => { var a = collection.Files.GetFile("a"); });
            AssertIndexOutOfRange(() => { var a = collection.Files.GetFiles("a"); });
            AssertIndexOutOfRange(() => { var a = collection.Files.GetEnumerator(); });
            AssertIndexOutOfRange(() => { var a = (collection.Files as IEnumerable).GetEnumerator(); });
        }

        private void AssertIndexOutOfRange(Action action) => AssertExcetion<IndexOutOfRangeException>(action);

        private void AssertExcetion<T>(Action action)
            where T : Exception
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Assert.True(ex is T);
            }
        }

        private HttpContext GetHttpCtx(bool notEmpty = true)
        {
            var httpCtx = new Fakes.FakeHttpContext();
            var request = (httpCtx.Request as Fakes.FakeHttpRequest);
            request.Method = "POST";
            var fileCollection = (request.Form.Files as Fakes.FakeFormFileCollection);
            if (notEmpty)
            {
                fileCollection.Data.Add("1", new Fakes.FakeFormFile());
                fileCollection.Data.Add("2", new Fakes.FakeFormFile());
            }
            return httpCtx;
        }

        private FormFileCollection CreateFormFileCollection(HttpRequest request)
        {
            var ctrl = typeof(FormFileCollection).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance);
            return ctrl.Single().Invoke(new object[] { request }) as FormFileCollection;
        }
    }
}
