using LiteApi.Contracts.Models;
using LiteApi.Services.ModelBinders;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class CollectionsQueryModelBinderTests
    {
        [Fact]
        public void CollectionsQueryModelBinder_MissingQuery_ReturnsEmptyArray()
        {
            var binder = new CollectionsQueryModelBinder();
            var request = GetRequest(null, null);
            var actionCtx = GetActionContext("sumArray");

            int[] result = binder.ParseParameterValue(request, actionCtx, actionCtx.Parameters.Single()) as int[];
            Assert.Empty(result);
        }

        [Fact]
        public void CollectionsQueryModelBinder_MissingQuery_ReturnsEmptyList()
        {
            var binder = new CollectionsQueryModelBinder();
            var request = GetRequest(null, null);
            var actionCtx = GetActionContext("sumList");

            List<int> result = binder.ParseParameterValue(request, actionCtx, actionCtx.Parameters.Single()) as List<int>;
            Assert.Empty(result);
        }

        [Fact]
        public void CollectionsQueryModelBinder_MissingQuery_ReturnsEmptyCollection()
        {
            var binder = new CollectionsQueryModelBinder();
            var request = GetRequest(null, null);
            var actionCtx = GetActionContext("sumCollection");

            IEnumerable<int> result = binder.ParseParameterValue(request, actionCtx, actionCtx.Parameters.Single()) as int[];
            Assert.Empty(result);
        }

        [Fact]
        public void CollectionsQueryModelBinder_EmptyQuery_ReturnsEmptyArray()
        {
            var binder = new CollectionsQueryModelBinder();
            var request = GetRequest("ints", new string[0]);
            var actionCtx = GetActionContext("sumArray");

            int[] result = binder.ParseParameterValue(request, actionCtx, actionCtx.Parameters.Single()) as int[];
            Assert.Empty(result);
        }

        [Fact]
        public void CollectionsQueryModelBinder_EmptyQuery_ReturnsEmptyList()
        {
            var binder = new CollectionsQueryModelBinder();
            var request = GetRequest("ints", new string[0]);
            var actionCtx = GetActionContext("sumList");

            List<int> result = binder.ParseParameterValue(request, actionCtx, actionCtx.Parameters.Single()) as List<int>;
            Assert.Empty(result);
        }

        [Fact]
        public void CollectionsQueryModelBinder_EmptyQuery_ReturnsEmptyCollection()
        {
            var binder = new CollectionsQueryModelBinder();
            var request = GetRequest("ints", new string[0]);
            var actionCtx = GetActionContext("sumCollection");

            IEnumerable<int> result = binder.ParseParameterValue(request, actionCtx, actionCtx.Parameters.Single()) as int[];
            Assert.Empty(result);
        }

        [Fact]
        public void CollectionsQueryModelBinder_NonEmptyQuery_CanParseArray()
        {
            var binder = new CollectionsQueryModelBinder();
            var request = GetRequest("ints", "1", "2", "3");
            var actionCtx = GetActionContext("sumArray");

            int[] result = binder.ParseParameterValue(request, actionCtx, actionCtx.Parameters.Single()) as int[];
            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void CollectionsQueryModelBinder_NonEmptyQuery_CanParseList()
        {
            var binder = new CollectionsQueryModelBinder();
            var request = GetRequest("ints", "1", "2", "3");
            var actionCtx = GetActionContext("sumList");

            List<int> result = binder.ParseParameterValue(request, actionCtx, actionCtx.Parameters.Single()) as List<int>;
            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void CollectionsQueryModelBinder_NonEmptyQuery_CanParseCollection()
        {
            var binder = new CollectionsQueryModelBinder();
            var request = GetRequest("ints", "1", "2", "3");
            var actionCtx = GetActionContext("sumCollection");

            int[] result = binder.ParseParameterValue(request, actionCtx, actionCtx.Parameters.Single()) as int[];
            Assert.Equal(new[] { 1, 2, 3 }, result);
        }

        [Fact]
        public void CollectionsQueryModelBinder_NonEmptyQuery_CanParseNullableIntArray()
        {
            var binder = new CollectionsQueryModelBinder();
            var request = GetRequest("ints", "1", "", "3");
            var actionCtx = GetActionContext("sumArrayNullable");

            int?[] result = binder.ParseParameterValue(request, actionCtx, actionCtx.Parameters.Single()) as int?[];
            Assert.Equal(new int?[] { 1, null, 3 }, result);
        }

        [Fact]
        public void CollectionsQueryModelBinder_NonEmptyQuery_CanParseNullableIntList()
        {
            var binder = new CollectionsQueryModelBinder();
            var request = GetRequest("ints", "1", "", "3");
            var actionCtx = GetActionContext("sumListNullable");

            List<int?> result = binder.ParseParameterValue(request, actionCtx, actionCtx.Parameters.Single()) as List<int?>;
            Assert.Equal(new int?[] { 1, null, 3 }, result);
        }

        [Fact]
        public void CollectionsQueryModelBinder_NonEmptyQuery_CanParseNullableIntCollection()
        {
            var binder = new CollectionsQueryModelBinder();
            var request = GetRequest("ints", "1", "", "3");
            var actionCtx = GetActionContext("sumCollectionNullable");

            int?[] result = binder.ParseParameterValue(request, actionCtx, actionCtx.Parameters.Single()) as int?[];
            Assert.Equal(new int?[] { 1, null, 3 }, result);
        }

        [Fact]
        public void CollectionsQueryModelBinder_NotCollectionParameter_ThrowsException()
        {
            var binder = new CollectionsQueryModelBinder();
            var request = GetRequest("ints", new string[0]);
            var actionCtx = GetActionContext("sumNotCollection");

            bool error = false;
            try
            {
                object result = binder.ParseParameterValue(request, actionCtx, actionCtx.Parameters.Single());
            }
            catch (Exception ex)
            {
                error = ex.Message.Contains("is not array, list or IEnumerable");
            }
            Assert.True(error);
        }
        
        private HttpRequest GetRequest(string key, params string[] values)
        {
            var request = Fakes.FakeHttpRequest.WithGetMethod();
            if (key != null)
            {
                request.AddQuery(key, values);
            }
            return request;
        }

        private ActionContext GetActionContext(string name)
        {
            var ctrlDiscoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.CollectionsQueryModelBinderController));
            return ctrlDiscoverer.GetControllers(null).Single().Actions.First(x => x.Name.ToLower() == name.ToLower());
        }
    }
}
