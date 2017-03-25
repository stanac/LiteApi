using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace LiteApi.Tests
{
    public class ParametersFromQueryTests : ParametersTestsBase
    {
        public override bool TestHeader => false;

        [Fact]
        public void QueryParameter_ParameterHasValue_ReturnsValue()
        {
            AssertParamParse("input", new[] { "132" }, typeof(int), 132);
        }

        [Fact]
        public void QueryParameter_NullableParameterHasValue_ReturnsValue()
        {
            AssertParamParse("input", new[] { "132" }, typeof(int?), 132);
        }

        [Fact]
        public void QueryParameter_NullableParameterHasNoValue_ReturnsNull()
        {
            AssertParamParse("input", new string[0], typeof(int?), null);
        }

        [Fact]
        public void QueryParameter_NullableNonStringParameterHasEmptyValue_ReturnsNull()
        {
            AssertParamParse("input", new[] { "" }, typeof(int?), null);
        }

        [Fact]
        public void QueryParameter_NullableStringParameterHasEmptyValue_ReturnsEmptyString()
        {
            AssertParamParse("input", new[] { "" }, typeof(string), "");
        }

        [Fact]
        public void QueryParameter_NullableParameterHasNoQuery_ThrowsException()
        {
            Action a = () => AssertParamParse(null, null, typeof(int?), null);
            AssertException(a, ex => ex.Message.Contains("does not contain value"));
        }

        [Fact]
        public void QueryParameter_NotNullableParameterHasNoQuery_ThrowsException()
        {
            Action a = () => AssertParamParse(null, null, typeof(int), null);
            AssertException(a, ex => ex.Message.Contains("does not contain value"));
        }

        [Fact]
        public void QueryParameter_NotNullableParameterHasNoValue_ThrowsException()
        {
            Action a = () => AssertParamParse("input", new string[0], typeof(int), null);
            AssertException(a, ex => ex.Message.Contains("Value is not provided for parameter"));
        }

        [Fact]
        public void QueryParameter_NotNullableParameterEmptyValue_ThrowsException()
        {
            Action a = () => AssertParamParse("input", new[] { "" }, typeof(int), null);
            AssertException(a, ex => ex.Message.Contains("Value is not provided for parameter"));
        }

        [Fact]
        public void QueryParameter_DefaultParameterHasValue_ReturnsValue()
        {
            AssertParamParse("input", new[] { "123" }, typeof(int), 123, true, 424);
        }

        [Fact]
        public void QueryParameter_DefaultParameterHasNoValue_ReturnsDefault()
        {
            AssertParamParse("input", new string[0], typeof(int), 424, true, 424);
        }

        [Fact]
        public void QueryParameter_DefaultParameterHasNoQuery_ReturnsDefault()
        {
            AssertParamParse(null, null, typeof(int), 424, true, 424);
        }

        [Fact]
        public void QueryParameter_DefaultParameterHasEmptyValue_ReturnsDefault()
        {
            AssertParamParse("input", new[] { "" }, typeof(int), 424, true, 424);
        }

        [Fact]
        public void QueryParameter_OverridenName_CanBeParsed()
        {
            AssertParamParse("x-override-input", new[] { "12" }, typeof(int), 12, overrideParamName: "x-override-input");
        }

    }
}
