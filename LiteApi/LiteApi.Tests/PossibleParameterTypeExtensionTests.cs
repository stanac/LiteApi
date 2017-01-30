using LiteApi.Contracts.Models.ActionMatchingByParameters;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using Xunit;

namespace LiteApi.Tests
{
    public class PossibleParameterTypeExtensionTests
    {
        private readonly string _floatString = "0.1";
        private readonly string _doubleString = "123546789012354678901235467890123546789012354678901235467890123546789012354678901235467890123546789012354678901235467890123546789012354678901235467890123546789012354678901235467890123546789012354678901235467890123546789012354678901235467890123546789012354678901235467890123546789012354678901235467890.132";
        private readonly string _decimalString = (decimal.MaxValue - 1.5M).ToString();
        private readonly string _uint16String = (UInt16.MaxValue - 1).ToString();
        private readonly string _uint32String = (UInt32.MaxValue - 1).ToString();
        private readonly string _uint64String = (UInt64.MaxValue - 1).ToString();
        private readonly string _byteString = (byte.MaxValue - 1).ToString();
        private readonly string _sbyteString = (sbyte.MaxValue - 1).ToString();
        private readonly string _boolString = "true";
        private readonly string _dateTimeString = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        private readonly string _guidString = Guid.NewGuid().ToString();
        private readonly string _stringString = "LiteApi";
        private readonly string _charString = "c";

        [Fact]
        public void HttpRequest_WithBody_ReturnsPosibleParameterTypeFromBody()
        {
            var request = Fakes.FakeHttpRequest.WithPostMethod();
            request.WriteBody("1");
            var possibleParamTypes = request.GetPossibleParameterTypes();
            Assert.True(possibleParamTypes.Any(x => x.Source == Contracts.Models.ParameterSources.Body));
        }

        [Fact]
        public void HttpRequest_With_FloatString_ReturnsFloatDecimalAndDouble()
            => AssertExpectedQueryTypes(_floatString, typeof(float), typeof(decimal), typeof(double));

        [Fact]
        public void HttpRequest_With_DecimalString_ReturnsDecimalAndDouble()
            => AssertExpectedQueryTypes(_decimalString, typeof(float), typeof(decimal), typeof(double));

        [Fact]
        public void HttpRequest_With_DoubleString_ReturnsDouble()
            => AssertExpectedQueryTypes(_doubleString, typeof(double));

        [Fact]
        public void HttpRequest_With_UInt16String_Returns_UInt16_UInt32_UInt64_Int32_and_Int64()
            => AssertExpectedQueryTypes(_uint16String, typeof(UInt16), typeof(UInt32), typeof(UInt64), typeof(Int32), typeof(Int64));
        
        [Fact]
        public void HttpRequest_With_UInt32String_Returns_UInt32_UInt64_and_Int64()
            => AssertExpectedQueryTypes(_uint32String, typeof(UInt32), typeof(UInt64), typeof(Int64));

        [Fact]
        public void HttpRequest_With_UInt64String_Returns_UInt64()
            => AssertExpectedQueryTypes(_uint64String, typeof(UInt64));

        [Fact]
        public void HttpRequest_With_ByteString_Returns_Byte()
            => AssertExpectedQueryTypes(_byteString, typeof(byte));

        [Fact]
        public void HttpRequest_With_SByteString_Returns_Byte_and_SByte()
            => AssertExpectedQueryTypes(_sbyteString, typeof(byte), typeof(sbyte));

        [Fact]
        public void HttpRequest_With_BoolString_Returns_Bool()
            => AssertExpectedQueryTypes(_boolString, typeof(bool));
        
        [Fact]
        public void HttpRequest_With_DateTimeString_Returns_DateTime()
            => AssertExpectedQueryTypes(_dateTimeString, typeof(DateTime));

        [Fact]
        public void HttpRequest_With_GuidString_Returns_Byte_and_SByte()
            => AssertExpectedQueryTypes(_guidString, typeof(Guid));

        [Fact]
        public void HttpRequest_With_StringString_Returns_String()
            => AssertExpectedQueryTypes(_stringString, typeof(string));

        [Fact]
        public void HttpRequest_With_CharString_Returns_Char()
            => AssertExpectedQueryTypes(_charString, typeof(char));

        private void AssertExpectedQueryTypes(string queryValue, params Type[] expectedTypes)
        {
            var request = GetRequestWithQuery(queryValue);
            var types = request.GetPossibleParameterTypes().ToArray().Single();
            
            AssertExpectedQueryTypes(types, expectedTypes);
        }

        private void AssertExpectedQueryTypes(PossibleParameterType possibleParamType, Type[] expectedTypes)
        {
            // every type can be a string
            if (!expectedTypes.Contains(typeof(string)))
            {
                var temp = expectedTypes.ToList();
                temp.Add(typeof(string));
                expectedTypes = temp.ToArray();
            }

            var possibleTypes = possibleParamType.PossibleTypes.OrderBy(x => x.TypePriority).Select(x => x.Type).ToList();

            var integerTypes = new [] { typeof(UInt16), typeof(UInt32), typeof(UInt64), typeof(Int16), typeof(Int32), typeof(Int64) };
            var floatingTypes = new[] { typeof(Decimal), typeof(Single), typeof(Double) };
            if (new [] { typeof(byte), typeof(sbyte) }.Any(x => expectedTypes.Contains(x)))
            {
                possibleTypes.RemoveAll(x => integerTypes.Contains(x));
                possibleTypes.RemoveAll(x => floatingTypes.Contains(x));
            }
            else if (integerTypes.Intersect(expectedTypes).Any())
            {
                possibleTypes.RemoveAll(x => floatingTypes.Contains(x));
            }

            Assert.Equal(expectedTypes, possibleTypes);
        }

        private HttpRequest GetRequestWithQuery(string value)
        {
            var request = Fakes.FakeHttpRequest.WithGetMethod();
            request.AddQuery("a", value);
            return request;
        }
    }
}
