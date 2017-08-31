using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using LiteApi.Services;
using LiteApi.Services.ModelBinders;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class ModelBinderCollectionTests
    {
        [Fact]
        public void ModelBinderCollection_NullSeralizer_ThrowsException()
        {
            bool error = false;
            try
            {
                var m = new ModelBinderCollection(null, Fakes.FakeServiceProvider.GetServiceProvider(), new Fakes.FakeDefaultLiteApiOptionsRetriever());
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void ModelBinderCollectionAddAdditionalQueryModelBinder_NullQueryModelBinder_ThrowsException()
        {
            bool error = false;
            var collection = new ModelBinderCollection(new JsonSerializer(), Fakes.FakeServiceProvider.GetServiceProvider(), new Fakes.FakeDefaultLiteApiOptionsRetriever());
            try
            {
                collection.AddAdditionalQueryModelBinder(null);
            }
            catch (ArgumentNullException)
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void ModelBinderCollection_AddingNewModelBinder_AddsSupportForNewType()
        {
            var mock = new Mock<IQueryModelBinder>();
            mock.SetupGet(x => x.SupportedTypes).Returns(new [] { typeof(ModelBinderCollectionTests) });

            var collection = new ModelBinderCollection(new JsonSerializer(), Fakes.FakeServiceProvider.GetServiceProvider(), new Fakes.FakeDefaultLiteApiOptionsRetriever());
            collection.AddAdditionalQueryModelBinder(mock.Object);
            var containsCustomType = collection.GetSupportedTypesFromUrl().Contains(typeof(ModelBinderCollectionTests));
            Assert.True(containsCustomType);
        }
        
        [Fact]
        public void ModelBinderCollectionDoesSupportType_TypeFromBody_IsSupported()
        {
            var collection = new ModelBinderCollection(new JsonSerializer(), Fakes.FakeServiceProvider.GetServiceProvider(), new Fakes.FakeDefaultLiteApiOptionsRetriever());
            bool supports = collection.DoesSupportType(typeof(ModelBinderCollectionTests), Contracts.Models.ParameterSources.Body);
            Assert.True(supports);
        }

        [Fact]
        public void ModelBinderCollectionDoesSupportType_FromUnknownSource_ThrowsException()
        {
            var collection = new ModelBinderCollection(new JsonSerializer(), Fakes.FakeServiceProvider.GetServiceProvider(), new Fakes.FakeDefaultLiteApiOptionsRetriever());
            bool error = false;
            try
            {
                collection.DoesSupportType(typeof(int), Contracts.Models.ParameterSources.Unknown);
            }
            catch
            {
                error = true;
            }
            Assert.True(error);
        }

        [Fact]
        public void ModelBinderGetParameterValues_UnsuportedActionParameterFromQuery_ThrowsException()
        {
            var collection = new ModelBinderCollection(new JsonSerializer(), Fakes.FakeServiceProvider.GetServiceProvider(), new Fakes.FakeDefaultLiteApiOptionsRetriever());
            bool error = false;
            try
            {
                var action = new ActionContext();
                action.Parameters = new[]
                {
                    new ActionParameter(action, collection)
                    {
                        Type = typeof(ModelBinderCollectionTests),
                        ParameterSource = ParameterSources.Query
                    }
                };
                collection.GetParameterValues(Fakes.FakeHttpRequest.WithGetMethod(), action);
            }
            catch (Exception ex)
            {
                error = ex.Message.Contains("No model binder supports type");
            }
            Assert.True(error);
        }

        [Fact]
        public void ModelBinderGetParameterValues_UnknownSourceActionParameter_ThrowsException()
        {
            var collection = new ModelBinderCollection(new JsonSerializer(), Fakes.FakeServiceProvider.GetServiceProvider(), new Fakes.FakeDefaultLiteApiOptionsRetriever());
            bool error = false;
            try
            {
                var action = new ActionContext();
                action.RouteSegments = new[] { new RouteSegment("action1") };
                action.Parameters = new[]
                {
                    new ActionParameter(action, collection)
                    {
                        Type = typeof(ModelBinderCollectionTests),
                        ParameterSource = ParameterSources.Unknown,
                        Name = "param1"
                    }
                };
                action.ParentController = new ControllerContext
                {
                    RouteAndName = "ctrl1"
                };
                collection.GetParameterValues(Fakes.FakeHttpRequest.WithGetMethod(), action);
            }
            catch (ArgumentException ex)
            {
                error = ex.Message.Contains("has unknown source");
            }
            Assert.True(error);
        }
    }
}
