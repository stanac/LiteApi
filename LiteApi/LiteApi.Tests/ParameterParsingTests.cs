using LiteApi.Contracts.Abstractions;
using LiteApi.Services;
using LiteApi.Services.ModelBinders;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LiteApi.Tests
{
    public class ParameterParsingTests
    {
        [Fact]
        public void ModelBinder_ArrayOfStrings_CanParse()
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.CollectionController));
            var ctrl = discoverer.GetControllers(null).Single();
            var action = ctrl.Actions.Single(x => x.Name == "get1");
            var mb = new ModelBinderCollection(new JsonSerializer());
            var request = Fakes.FakeHttpRequest.WithGetMethod()
                .AddQuery("data", "1", "1", "2", "5678", "abcd");
            object[] parameters = mb.GetParameterValues(request, action);
            Assert.Equal(1, parameters.Length);
            var arrayFromParams = (string[])parameters[0];
            Assert.Equal(new[] { "1", "1", "2", "5678", "abcd" }, arrayFromParams);
        }

        [Fact]
        public void ModelBinder_ListOfIntegers_CanParse()
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.CollectionController));
            var ctrl = discoverer.GetControllers(null).Single();
            var action = ctrl.Actions.Single(x => x.Name == "get2");
            var mb = new ModelBinderCollection(new JsonSerializer());
            var request = Fakes.FakeHttpRequest.WithGetMethod()
                .AddQuery("data", "1", "1", "2", "5678");
            object[] parameters = mb.GetParameterValues(request, action);
            Assert.Equal(1, parameters.Length);
            var listFromParams = (List<int>)parameters[0];
            Assert.Equal(new List<int>() { 1, 1, 2, 5678 }, listFromParams);
        }

        [Fact]
        public void ModelBinder_ListOfNullableIntegers_CanParse()
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.CollectionController));
            var ctrl = discoverer.GetControllers(null).Single();
            var action = ctrl.Actions.Single(x => x.Name == "get3");
            var mb = new ModelBinderCollection(new JsonSerializer());
            var request = Fakes.FakeHttpRequest.WithGetMethod()
                .AddQuery("data", "1", "", "1", "2", "5678");
            object[] parameters = mb.GetParameterValues(request, action);
            Assert.Equal(1, parameters.Length);
            var listFromParams = (List<int?>)parameters[0];
            Assert.Equal(new List<int?>() { 1, null, 1, 2, 5678 }, listFromParams);
        }

        [Fact]
        public void ModelBinder_ListOfNullableGuids_CanParse()
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.CollectionController));
            var ctrl = discoverer.GetControllers(null).Single();
            var action = ctrl.Actions.Single(x => x.Name == "get4");
            var mb = new ModelBinderCollection(new JsonSerializer());
            var request = Fakes.FakeHttpRequest.WithGetMethod()
                .AddQuery("data", "b5477041395b47e8ad52605a42462936", "", "{aa2ec3ca-c3e7-4695-be0c-0c33e527515b}", "d29f0102-2c99-4033-ad65-672f6db25a23");
            object[] parameters = mb.GetParameterValues(request, action);
            Assert.Equal(1, parameters.Length);
            var listFromParams = (List<Guid?>)parameters[0];
            var expected = new List<Guid?>()
            {
                Guid.Parse("b5477041395b47e8ad52605a42462936"),
                null,
                Guid.Parse("{aa2ec3ca-c3e7-4695-be0c-0c33e527515b}"),
                Guid.Parse("d29f0102-2c99-4033-ad65-672f6db25a23")
            };
            Assert.Equal(expected, listFromParams);
        }

        [Fact]
        public void ModelBinder_IEnumerableOfChars_CanParse()
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.CollectionController));
            var ctrl = discoverer.GetControllers(null).Single();
            var action = ctrl.Actions.Single(x => x.Name == "get5");
            var mb = new ModelBinderCollection(new JsonSerializer());
            var request = Fakes.FakeHttpRequest.WithGetMethod()
                .AddQuery("data", "a", "b", "c", "d", "e");
            object[] parameters = mb.GetParameterValues(request, action);
            Assert.Equal(1, parameters.Length);
            var collectionFromParams = (IEnumerable<char>)parameters[0];
            Assert.Equal(new[] { 'a', 'b', 'c', 'd', 'e' }.AsEnumerable(), collectionFromParams);
        }

        [Fact]
        public void ModelBinder_Dictionary_CanParse()
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.CollectionController));
            var ctrl = discoverer.GetControllers(null).Single();
            var action = ctrl.Actions.Single(x => x.Name == "get6");
            var mb = new ModelBinderCollection(new JsonSerializer());
            var request = Fakes.FakeHttpRequest.WithGetMethod()
                .AddQuery("data.1", "a")
                .AddQuery("data.2", "b")
                .AddQuery("data.3", "c")
                .AddQuery("data.4", "d");
            object[] parameters = mb.GetParameterValues(request, action);
            Assert.Equal(1, parameters.Length);
            var collectionFromParams = (IDictionary<int, string>)parameters[0];
            var expected = new Dictionary<int, string>
            {
                { 1, "a" }, { 2, "b" }, { 3, "c" }, { 4, "d" }
            } as IDictionary<int, string>;
            Assert.Equal(expected, collectionFromParams);
        }

        [Fact]
        public void ModelBinder_ComplexFromBodyParameter_CanParse()
        {
            IControllerDiscoverer discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.CollectionController));
            var ctrl = discoverer.GetControllers(null).Single();
            var action = ctrl.Actions.Single(x => x.Name == "post7");
            var mb = new ModelBinderCollection(new JsonSerializer());
            var request = Fakes.FakeHttpRequest.WithPostMethod()
                .WriteBody(@"
[
    { ""Item1"": 0, ""Item2"": ""0"" },
    { ""Item1"": 1, ""Item2"": ""2"" },
    { ""Item1"": 3, ""Item2"": ""44"" },
]
");
            object[] parameters = mb.GetParameterValues(request, action);
            Assert.Equal(1, parameters.Length);
            var collectionFromParams = (IEnumerable<Tuple<int, string>>)parameters[0];
            Tuple<int, string>[] expected = { Tuple.Create(0, "0"), Tuple.Create(1, "2"), Tuple.Create(3, "44") };
            Assert.Equal(expected, collectionFromParams);
        }
    }
}
