using LiteApi.Services;
using Xunit;

namespace LiteApi.Tests
{
    public class JsonSerializerTests
    {
        [Fact]
        public void DefaultJsonSerializer_CanSeralizeAndDeserailize()
        {
            JsonSerializer s = new JsonSerializer();
            var model = new TestModel();
            string json = s.Serialize(model);
            TestModel obj1 = s.Deserialize<TestModel>(json);
            TestModel obj2 = (TestModel)s.Deserialize(json, typeof(TestModel));
            Assert.Equal(model.I, obj1.I);
            Assert.Equal(model.J, obj1.J);
            Assert.Equal(model.I, obj2.I);
            Assert.Equal(model.J, obj2.J);
        }

        private class TestModel
        {
            public int I { get; set; } = 3;
            public string J { get; set; } = "Test";
        }
    }
}
