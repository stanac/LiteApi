using System;
using System.Collections.Generic;
using System.Linq;
using LiteApi.Contracts.Abstractions;
using System.Reflection;
using System.Collections;

namespace LiteApi.Contracts.Models
{
    /// <summary>
    /// Metadata for action parameter that is received by the API.
    /// </summary>
    public class ActionParameter
    {
        private bool _isTypeNullable;
        private bool _isTypeArray;
        private bool _isTypeList;
        private Type _collectionElementType;
        private bool _isCollectionElementTypeNullable;
        private Type _type;

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
        
        /// <summary>
        /// Gets or sets the reflected parameter type.
        /// </summary>
        /// <value>
        /// The reflected parameter type.
        /// </value>
        public Type Type
        {
            get { return _type; }
            set
            {
                string debugStr = value.ToString();
                _type = value;
                TypeInfo info = value.GetTypeInfo();
                Type nullableArgument;
                if (_isTypeNullable = info.IsNullable(out nullableArgument))
                {
                    _type = nullableArgument;
                }
                if (info.IsGenericType && info.GetGenericTypeDefinition() == typeof(List<>))
                {
                    _isTypeList = true;
                    _collectionElementType = value.GenericTypeArguments.Single();
                }
                else if (value.IsArray)
                {
                    _isTypeArray = true;
                    _collectionElementType = value.GetElementType();
                }
                if (IsArrayOrList)
                {
                    TypeInfo collectionElementInfo = _collectionElementType.GetTypeInfo();
                    if (_isCollectionElementTypeNullable = _collectionElementType.GetTypeInfo().IsNullable(out nullableArgument))
                    {
                        _collectionElementType = nullableArgument;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this action parameter is array.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this action parameter is array; otherwise, <c>false</c>.
        /// </value>
        public bool IsArray => _isTypeArray;

        /// <summary>
        /// Gets a value indicating whether this action parameter is list.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this action parameter is list; otherwise, <c>false</c>.
        /// </value>
        public bool IsList => _isTypeList;

        /// <summary>
        /// Gets a value indicating whether this action parameter is array or list.
        /// </summary>
        /// <value>
        /// <c>true</c> if this action parameter is array or list; otherwise, <c>false</c>.
        /// </value>
        public bool IsArrayOrList => _isTypeArray || _isTypeList;

        /// <summary>
        /// Gets a value indicating whether this action parameter is nullable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this action parameter is nullable; otherwise, <c>false</c>.
        /// </value>
        public bool IsNullable => _isTypeNullable;

        /// <summary>
        /// Gets a value indicating whether the parameter is complex (not in <see cref="SupportedTypesFromUrl"/>).
        /// </summary>
        /// <value>
        /// <c>true</c> if the parameter is complex; otherwise, <c>false</c>.
        /// </value>
        public bool IsComplex => !SupportedTypesFromUrl.Contains(Type);

        #region SupportedTypesFromUrl

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

            typeof (bool[][]),
            typeof (string[]),
            typeof (char[]),
            typeof (Int16[]),
            typeof (Int32[]),
            typeof (Int64[]),
            typeof (UInt16[]),
            typeof (UInt32[]),
            typeof (UInt64[]),
            typeof (Byte[]),
            typeof (SByte[]),
            typeof (decimal[]),
            typeof (float[]),
            typeof (double[]),
            typeof (DateTime[]),
            typeof (Guid[]),

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
            typeof (Guid?),

            typeof (bool?[]),
            // typeof (string?[]),
            typeof (char?[]),
            typeof (Int16?[]),
            typeof (Int32?[]),
            typeof (Int64?[]),
            typeof (UInt16?[]),
            typeof (UInt32?[]),
            typeof (UInt64?[]),
            typeof (Byte?[]),
            typeof (SByte?[]),
            typeof (decimal?[]),
            typeof (float?[]),
            typeof (double?[]),
            typeof (DateTime?[]),
            typeof (Guid?[]),

            typeof (List<bool>),
            typeof (List<string>),
            typeof (List<char>),
            typeof (List<Int16>),
            typeof (List<Int32>),
            typeof (List<Int64>),
            typeof (List<UInt16>),
            typeof (List<UInt32>),
            typeof (List<UInt64>),
            typeof (List<Byte>),
            typeof (List<SByte>),
            typeof (List<decimal>),
            typeof (List<float>),
            typeof (List<double>),
            typeof (List<DateTime>),
            typeof (List<Guid>),

            typeof (List<bool?>),
            //typeof (List<string?>),
            typeof (List<char?>),
            typeof (List<Int16?>),
            typeof (List<Int32?>),
            typeof (List<Int64?>),
            typeof (List<UInt16?>),
            typeof (List<UInt32?>),
            typeof (List<UInt64?>),
            typeof (List<Byte?>),
            typeof (List<SByte?>),
            typeof (List<decimal?>),
            typeof (List<float?>),
            typeof (List<double?>),
            typeof (List<DateTime?>),
            typeof (List<Guid?>)
        };

        #endregion
                
        /// <summary>
        /// Parses the string value.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <returns>Parsed string value</returns>
        /// <exception cref="System.ArgumentException"></exception>
        public object ParseValue(string[] value)
        {
            if (ParameterSource == ParameterSources.Query)
            {
                return ParseQueryValue(value);
            }
            if (ParameterSource == ParameterSources.Body)
            {
                return ResolveJsonSerializer().Deserialize(value.Single(), Type);
            }
            throw new ArgumentException($"Parameter {Name} has unknown source. " + Attributes.AttributeConventions.ErrorResolutionSuggestion);
        }

        /// <summary>
        /// Gets the supported types.
        /// </summary>
        /// <returns>Collection of supported <see cref="Type"/> from URL</returns>
        public static IEnumerable<Type> GetSupportedTypesFromUrl()
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
        private object ParseQueryValue(string[] values)
        {
            if (IsArrayOrList)
            {
                return ParseCollectionParameter(values);
            }
            string value = values.FirstOrDefault();

            if (value == null && HasDefaultValue)
            {
                return DefaultValue;
            }
            
            return ParseSingleQueryValue(value, Type, _isTypeNullable, Name);
        }

        private static object ParseSingleQueryValue(string value, Type type, bool isNullable, string parameterName)
        {
            if (type == typeof(string))
            {
                return value;
            }

            if (string.IsNullOrEmpty(value))
            {
                if (isNullable)
                {
                    return null;
                }
                throw new ArgumentException($"Value is not provided for parameter: {parameterName}");
            }

            if (type == typeof(bool)) return bool.Parse(value);
            if (type == typeof(char)) return char.Parse(value);
            if (type == typeof(Guid)) return Guid.Parse(value);
            if (type == typeof(Int16)) return Int16.Parse(value);
            if (type == typeof(Int32)) return Int32.Parse(value);
            if (type == typeof(Int64)) return Int64.Parse(value);
            if (type == typeof(UInt16)) return UInt16.Parse(value);
            if (type == typeof(UInt32)) return UInt32.Parse(value);
            if (type == typeof(UInt64)) return UInt64.Parse(value);
            if (type == typeof(Byte)) return Byte.Parse(value);
            if (type == typeof(SByte)) return SByte.Parse(value);
            if (type == typeof(decimal)) return decimal.Parse(value);
            if (type == typeof(float)) return float.Parse(value);
            if (type == typeof(double)) return double.Parse(value);
            if (type == typeof(DateTime)) return DateTime.Parse(value);
            if (type == typeof(Guid)) return Guid.Parse(value);
            
            //if (type == typeof(bool?)) return bool.Parse(value);
            //if (type == typeof(char?)) return char.Parse(value);
            //if (type == typeof(Guid?)) return Guid.Parse(value);
            //if (type == typeof(Int16?)) return Int16.Parse(value);
            //if (type == typeof(Int32?)) return Int32.Parse(value);
            //if (type == typeof(Int64?)) return Int64.Parse(value);
            //if (type == typeof(UInt16?)) return UInt16.Parse(value);
            //if (type == typeof(UInt32?)) return UInt32.Parse(value);
            //if (type == typeof(UInt64?)) return UInt64.Parse(value);
            //if (type == typeof(Byte?)) return Byte.Parse(value);
            //if (type == typeof(SByte?)) return SByte.Parse(value);
            //if (type == typeof(decimal?)) return decimal.Parse(value);
            //if (type == typeof(float?)) return float.Parse(value);
            //if (type == typeof(double?)) return double.Parse(value);
            //if (type == typeof(DateTime?)) return DateTime.Parse(value);
            //if (type == typeof(Guid?)) return Guid.Parse(value);

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

        private object ParseCollectionParameter(string[] values)
        {
            if (_isTypeArray)
            {
                Array parsedValues = Array.CreateInstance(_collectionElementType, values.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    parsedValues.SetValue(ParseSingleQueryValue(values[i], _collectionElementType, _isCollectionElementTypeNullable, Name), i);
                }
                return parsedValues;
            }
            if (_isTypeList)
            {
                IList parsedValues = Activator.CreateInstance(Type) as IList;
                foreach (var value in values)
                {
                    parsedValues.Add(ParseSingleQueryValue(value, _collectionElementType, _isCollectionElementTypeNullable, Name));
                }
                return parsedValues;
            }

            throw new InvalidOperationException("Action parameter is not list or array");
        }
        
    }
}
