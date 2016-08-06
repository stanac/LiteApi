using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LiteApi.Contracts
{
    public class ActionParameter
    {
        public string Name { get; set; }
        public bool IsQuery { get; set; }
        public object DefaultValue { get; set; }
        public bool HasDefaultValue { get; set; }
        private bool _isTypeNullable;

        public Type Type
        {
            get { return _type; }
            set
            {
                _type = value;
                _isTypeNullable = Type.IsGenericParameter && Type.GetGenericTypeDefinition() == typeof(Nullable<>);
            }
        }

        private static readonly Type[] SupportedTypes = {
            typeof (bool),
            typeof (string),
            typeof (char),
            typeof (Guid),
            typeof (Int16),
            typeof (Int32),
            typeof (Int64),
            typeof (UInt16),
            typeof (UInt32),
            typeof (UInt64),
            typeof (Byte),
            typeof (SByte),
            typeof (decimal),
            typeof (float),
            typeof (double),

            typeof (bool?),
            // typeof (string?),
            typeof (char?),
            typeof (Guid?),
            typeof (Int16?),
            typeof (Int32?),
            typeof (Int64?),
            typeof (UInt16?),
            typeof (UInt32?),
            typeof (UInt64?),
            typeof (Byte?),
            typeof (SByte?),
            typeof (decimal?),
            typeof (float?),
            typeof (double?)
        };

        private Type _type;

        public object ParseValue(string value)
        {
            if (IsQuery)
            {
                return ParseQueryValue(value);
            }
            if (value == null)
            {
                return null;
            }
            return JsonConvert.DeserializeObject(value, Type);
        }

        public static IEnumerable<Type> GetSupportedType()
        {
            return SupportedTypes.Select(x => x);
        }

        private object ParseQueryValue(string value)
        {
            if (value == null && HasDefaultValue)
            {
                return DefaultValue;
            }

            if (Type == typeof(string))
            {
                return value;
            }

            if (value == null && _isTypeNullable)
            {
                return true;
            }

            if (value == null /*&& !_isTypeNullable*/)
            {
                throw new ArgumentException($"Value is not provided for parameter: {Name}");
            }

            if (Type == typeof(bool)) return bool.Parse(value);
            if (Type == typeof(char)) return char.Parse(value);
            if (Type == typeof(Guid)) return Guid.Parse(value);
            if (Type == typeof(Int16)) return Int16.Parse(value);
            if (Type == typeof(Int32)) return Int32.Parse(value);
            if (Type == typeof(Int64)) return Int64.Parse(value);
            if (Type == typeof(UInt16)) return UInt16.Parse(value);
            if (Type == typeof(UInt32)) return UInt32.Parse(value);
            if (Type == typeof(UInt64)) return UInt64.Parse(value);
            if (Type == typeof(Byte)) return Byte.Parse(value);
            if (Type == typeof(SByte)) return SByte.Parse(value);
            if (Type == typeof(decimal)) return decimal.Parse(value);
            if (Type == typeof(float)) return float.Parse(value);
            if (Type == typeof(double)) return double.Parse(value);
            if (Type == typeof(bool?)) return bool.Parse(value);
            if (Type == typeof(char?)) return char.Parse(value);
            if (Type == typeof(Guid?)) return Guid.Parse(value);
            if (Type == typeof(Int16?)) return Int16.Parse(value);
            if (Type == typeof(Int32?)) return Int32.Parse(value);
            if (Type == typeof(Int64?)) return Int64.Parse(value);
            if (Type == typeof(UInt16?)) return UInt16.Parse(value);
            if (Type == typeof(UInt32?)) return UInt32.Parse(value);
            if (Type == typeof(UInt64?)) return UInt64.Parse(value);
            if (Type == typeof(Byte?)) return Byte.Parse(value);
            if (Type == typeof(SByte?)) return SByte.Parse(value);
            if (Type == typeof(decimal?)) return decimal.Parse(value);
            if (Type == typeof(float?)) return float.Parse(value);
            if (Type == typeof(double?)) return double.Parse(value);

            throw new ArgumentOutOfRangeException();
        }

        public void ThrowOnInvalidType()
        {
            if (IsQuery && !SupportedTypes.Contains(Type))
            {
                throw new InvalidOperationException("Type is not supported for query parameters, check NameFront.HttpServer.ActionRequestHandler.ActionParameter.GetSupportedType() for supported types in query parameters");
            }
        }
    }
}
