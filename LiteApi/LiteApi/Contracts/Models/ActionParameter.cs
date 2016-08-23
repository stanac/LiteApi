using System;
using System.Collections.Generic;
using System.Linq;
using LiteApi.Contracts.Abstractions;

namespace LiteApi.Contracts.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionParameter
    {
        /// <summary>
        /// Gets or sets the JSON serializer factory.
        /// </summary>
        /// <value>
        /// The JSON serializer factory.
        /// </value>
        public static Func<IJsonSerializer> ResolveJsonSerializer { get; set; } = () => LiteApiMiddleware.Options.JsonSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionParameter"/> class.
        /// </summary>
        /// <param name="parentActionCtx">The parent action CTX.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ActionParameter(ActionContext parentActionCtx)
        {
            if (parentActionCtx == null) throw new ArgumentNullException(nameof(parentActionCtx));
            ParentActionCtx = parentActionCtx;
        }

        /// <summary>
        /// Gets or sets the parent action CTX.
        /// </summary>
        /// <value>
        /// The parent action CTX.
        /// </value>
        public ActionContext ParentActionCtx { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parameter source (body, URL or unknown).
        /// </summary>
        /// <value>
        /// The parameter source.
        /// </value>
        public ParameterSources ParameterSource { get; set; } = ParameterSources.Unknown;

        /// <summary>
        /// Gets or sets the default value of the parameters.
        /// </summary>
        /// <value>
        /// The default value of the parameter.
        /// </value>
        public object DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has default value.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has default value; otherwise, <c>false</c>.
        /// </value>
        public bool HasDefaultValue { get; set; }
        
        private bool _isTypeNullable;

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type
        {
            get { return _type; }
            set
            {
                _type = value;
                _isTypeNullable = Type.IsGenericParameter && Type.GetGenericTypeDefinition() == typeof(Nullable<>);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the parameter is complex (not in <see cref="SupportedTypesFromUrl"/>).
        /// </summary>
        /// <value>
        /// <c>true</c> if the parameter is complex; otherwise, <c>false</c>.
        /// </value>
        public bool IsComplex => !SupportedTypesFromUrl.Contains(Type);


        // TODO: add support for arrays

        /// <summary>
        /// The supported types from URL
        /// </summary>
        private static readonly Type[] SupportedTypesFromUrl = {
            typeof (bool),
            typeof (string),
            typeof (char),
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
            typeof (DateTime),
            typeof (Guid),

            typeof (bool?),
            // typeof (string?),
            typeof (char?),
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
            typeof (double?),
            typeof (DateTime?),
            typeof (Guid?)
        };
        
        private Type _type;

        /// <summary>
        /// Parses the string value.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <returns>Parsed string value in for of an object</returns>
        /// <exception cref="System.ArgumentException"></exception>
        public object ParseValue(string value)
        {
            if (ParameterSource == ParameterSources.Query)
            {
                return ParseQueryValue(value);
            }
            if (ParameterSource == ParameterSources.Body)
            {
                return ResolveJsonSerializer().Deserialize(value, Type);
            }
            throw new ArgumentException($"Parameter {Name} has unknown source. " + Attributes.AttributeConventions.ErrorResolutionSuggestion);
        }

        /// <summary>
        /// Gets the supported types.
        /// </summary>
        /// <returns>Collection of supported <see cref="Type"/></returns>
        public static IEnumerable<Type> GetSupportedType()
        {
            return SupportedTypesFromUrl.Select(x => x);
        }

        /// <summary>
        /// Parses string value from query.
        /// </summary>
        /// <param name="value">String value from query.</param>
        /// <returns>Parse value in form of an object</returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
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
            if (Type == typeof(DateTime)) return DateTime.Parse(value);
            if (Type == typeof(Guid)) return Guid.Parse(value);
            
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
            if (Type == typeof(DateTime?)) return DateTime.Parse(value);
            if (Type == typeof(Guid?)) return Guid.Parse(value);

            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// Determines whether if type is supported.
        /// </summary>
        /// <returns><c>true</c> if type is supported, otherwise <c>false</c>.</returns>
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
