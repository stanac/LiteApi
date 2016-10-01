using LiteApi.Services.Validators;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace LiteApi.Tests
{
    public class RouteParameterSupportedTypesTests
    {
        [Fact]
        public void ParameterOfType_bool_IsSupported()
        {
            AssertParameterOfTypeIsSupported<bool>();
        }

        [Fact]
        public void ParameterOfType_string_IsSupported()
        {
            AssertParameterOfTypeIsSupported<string>();
        }

        [Fact]
        public void ParameterOfType_char_IsSupported()
        {
            AssertParameterOfTypeIsSupported<char>();
        }

        [Fact]
        public void ParameterOfType_Int16_IsSupported()
        {
            AssertParameterOfTypeIsSupported<Int16>();
        }

        [Fact]
        public void ParameterOfType_Int32_IsSupported()
        {
            AssertParameterOfTypeIsSupported<Int32>();
        }

        [Fact]
        public void ParameterOfType_Int64_IsSupported()
        {
            AssertParameterOfTypeIsSupported<Int64>();
        }

        [Fact]
        public void ParameterOfType_UInt16_IsSupported()
        {
            AssertParameterOfTypeIsSupported<UInt16>();
        }

        [Fact]
        public void ParameterOfType_UInt32_IsSupported()
        {
            AssertParameterOfTypeIsSupported<UInt32>();
        }

        [Fact]
        public void ParameterOfType_UInt64_IsSupported()
        {
            AssertParameterOfTypeIsSupported<UInt64>();
        }

        [Fact]
        public void ParameterOfType_Byte_IsSupported()
        {
            AssertParameterOfTypeIsSupported<Byte>();
        }

        [Fact]
        public void ParameterOfType_SByte_IsSupported()
        {
            AssertParameterOfTypeIsSupported<SByte>();
        }

        [Fact]
        public void ParameterOfType_decimal_IsSupported()
        {
            AssertParameterOfTypeIsSupported<decimal>();
        }

        [Fact]
        public void ParameterOfType_float_IsSupported()
        {
            AssertParameterOfTypeIsSupported<float>();
        }

        [Fact]
        public void ParameterOfType_double_IsSupported()
        {
            AssertParameterOfTypeIsSupported<double>();
        }

        [Fact]
        public void ParameterOfType_DateTime_IsSupported()
        {
            AssertParameterOfTypeIsSupported<DateTime>();
        }

        [Fact]
        public void ParameterOfType_Guid_IsSupported()
        {
            AssertParameterOfTypeIsSupported<Guid>();
        }

        [Fact]
        public void ParameterOfType_bool_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<bool?>();
        }

        [Fact]
        public void ParameterOfType_char_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<char?>();
        }

        [Fact]
        public void ParameterOfType_Int16_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<Int16?>();
        }

        [Fact]
        public void ParameterOfType_Int32_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<Int32?>();
        }

        [Fact]
        public void ParameterOfType_Int64_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<Int64?>();
        }

        [Fact]
        public void ParameterOfType_UInt16_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<UInt16?>();
        }

        [Fact]
        public void ParameterOfType_UInt32_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<UInt32?>();
        }

        [Fact]
        public void ParameterOfType_UInt64_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<UInt64?>();
        }

        [Fact]
        public void ParameterOfType_Byte_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<Byte?>();
        }

        [Fact]
        public void ParameterOfType_SByte_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<SByte?>();
        }

        [Fact]
        public void ParameterOfType_decimal_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<decimal?>();
        }

        [Fact]
        public void ParameterOfType_float_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<float?>();
        }

        [Fact]
        public void ParameterOfType_double_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<double?>();
        }

        [Fact]
        public void ParameterOfType_DateTime_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<DateTime?>();
        }

        [Fact]
        public void ParameterOfType_Guid_Nullable_IsNotSupported()
        {
            AssertParameterOfTypeIsSupported<Guid?>();
        }

        private void AssertParameterOfTypeIsSupported<T>()
        {
            Type type;
            Type nullableArg;
            bool isNullable = false;
            if (typeof(T).GetTypeInfo().IsNullable(out nullableArg))
            {
                type = nullableArg;
                isNullable = true;
            }
            else
            {
                type = typeof(T);
            }
            
            string actionName = "Action_" + type.Name + (isNullable ? "_Nullable" : "");

            var discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.RouteSupportedParametersController));
            var ctrl = discoverer.GetControllers(null).Single();
            var action = ctrl.Actions.Single(x => x.Name == actionName.ToLower());

            var errors = new ParametersValidator().GetParametersErrors(action);
            Assert.Equal(isNullable, errors.Any());
        }
    }
}
