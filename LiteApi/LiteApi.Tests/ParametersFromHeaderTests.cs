using System;
using Xunit;

namespace LiteApi.Tests
{
    public class ParametersFromHeaderTests: ParametersTestsBase
    {
        public override bool TestHeader => true;

        [Fact]
        public void HeaderParameter_ParameterHasValue_ReturnsValue()
        {
            AssertParamParse("input", new[] { "132" }, typeof(int), 132);
        }

        [Fact]
        public void HeaderParameter_NullableParameterHasValue_ReturnsValue()
        {
            AssertParamParse("input", new[] { "132" }, typeof(int?), 132);
        }

        [Fact]
        public void HeaderParameter_NullableParameterHasNoValue_ReturnsNull()
        {
            AssertParamParse("input", new string[0], typeof(int?), null);
        }

        [Fact]
        public void HeaderParameter_NullableNonStringParameterHasEmptyValue_ReturnsNull()
        {
            AssertParamParse("input", new[] { "" }, typeof(int?), null);
        }

        [Fact]
        public void HeaderParameter_NullableStringParameterHasEmptyValue_ReturnsEmptyString()
        {
            AssertParamParse("input", new[] { "" }, typeof(string), "");
        }

        [Fact]
        public void HeaderParameter_NullableParameterHasNoHeader_ThrowsException()
        {
            Action a = () => AssertParamParse(null, null, typeof(int?), null);
            AssertException(a, ex => ex.Message.Contains("does not contain value"));
        }

        [Fact]
        public void HeaderParameter_NotNullableParameterHasNoHeader_ThrowsException()
        {
            Action a = () => AssertParamParse(null, null, typeof(int), null);
            AssertException(a, ex => ex.Message.Contains("does not contain value"));
        }

        [Fact]
        public void HeaderParameter_NotNullableParameterHasNoValue_ThrowsException()
        {
            Action a = () => AssertParamParse("input", new string[0], typeof(int), null);
            AssertException(a, ex => ex.Message.Contains("Value is not provided for parameter"));
        }

        [Fact]
        public void HeaderParameter_NotNullableParameterEmptyValue_ThrowsException()
        {
            Action a = () => AssertParamParse("input", new[] { "" }, typeof(int), null);
            AssertException(a, ex => ex.Message.Contains("Value is not provided for parameter"));
        }

        [Fact]
        public void HeaderParameter_DefaultParameterHasValue_ReturnsValue()
        {
            AssertParamParse("input", new[] { "123" }, typeof(int), 123, true, 424);
        }

        [Fact]
        public void HeaderParameter_DefaultParameterHasNoValue_ReturnsDefault()
        {
            AssertParamParse("input", new string[0], typeof(int), 424, true, 424);
        }

        [Fact]
        public void HeaderParameter_DefaultParameterHasNoHeader_ReturnsDefault()
        {
            AssertParamParse(null, null, typeof(int), 424, true, 424);
        }

        [Fact]
        public void HeaderParameter_DefaultParameterHasEmptyValue_ReturnsDefault()
        {
            AssertParamParse("input", new[] { "" }, typeof(int), 424, true, 424);
        }

        [Fact]
        public void HeaderParameter_OverridenName_CanBeParsed()
        {
            AssertParamParse("x-override-input", new[] { "12" }, typeof(int), 12, overrideParamName: "x-override-input");
        }

    }
}
