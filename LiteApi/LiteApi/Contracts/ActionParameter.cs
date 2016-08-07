using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LiteApi.Contracts
{
    public class ActionParameter
    {
        public ActionParameter(ActionContext parentActionCtx)
        {
            if (parentActionCtx == null) throw new ArgumentNullException(nameof(parentActionCtx));
            ParentActionCtx = parentActionCtx;
        }

        public ActionContext ParentActionCtx { get; set; }
        public string Name { get; set; }
        public ParameterSources ParameterSource { get; set; }
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

        public bool IsComplex => SupportedTypesFromUrl.Contains(Type);

        private static readonly Type[] SupportedTypesFromUrl = {
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
            if (ParameterSource == ParameterSources.Query)
            {
                return ParseQueryValue(value);
            }
            if (ParameterSource == ParameterSources.Body)
            {
                return JsonConvert.DeserializeObject(value, Type);
            }
            throw new ArgumentException($"Parameter {Name} has unknown source. " + Attributes.AttributeConventions.ErrorResolutionSuggestion);
        }

        public static IEnumerable<Type> GetSupportedType()
        {
            return SupportedTypesFromUrl.Select(x => x);
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
                return null;
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

        public bool IsTypeSupported()
        {
            if (ParameterSource == ParameterSources.Query)
            {
                return SupportedTypesFromUrl.Contains(Type);
            }
            return true;
        }
    }
}
