using System;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;
using Microsoft.Extensions.Primitives;
using LiteApi.Contracts.Abstractions;

namespace LiteApi.Services.ModelBinders
{
    /// <summary>
    /// Query model binder that supports lists and arrays
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IQueryModelBinder" />
    public class CollectionsQueryModelBinder : BasicQueryModelBinder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionsQueryModelBinder"/> class.
        /// </summary>
        /// <param name="liteApiOptionsRetriever">The lite API options retriever.</param>
        public CollectionsQueryModelBinder(ILiteApiOptionsRetriever liteApiOptionsRetriever)
            : base(liteApiOptionsRetriever)
        {
        }

        #region _supportedTypes

        private static readonly Type[] _supportedTypes =
        {
            typeof (bool[]),
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
            typeof (List<Guid?>),

            typeof (IEnumerable<bool>),
            typeof (IEnumerable<string>),
            typeof (IEnumerable<char>),
            typeof (IEnumerable<Int16>),
            typeof (IEnumerable<Int32>),
            typeof (IEnumerable<Int64>),
            typeof (IEnumerable<UInt16>),
            typeof (IEnumerable<UInt32>),
            typeof (IEnumerable<UInt64>),
            typeof (IEnumerable<Byte>),
            typeof (IEnumerable<SByte>),
            typeof (IEnumerable<decimal>),
            typeof (IEnumerable<float>),
            typeof (IEnumerable<double>),
            typeof (IEnumerable<DateTime>),
            typeof (IEnumerable<Guid>),

            typeof (IEnumerable<bool?>),
            //typeof (IEnumerable<string?>),
            typeof (IEnumerable<char?>),
            typeof (IEnumerable<Int16?>),
            typeof (IEnumerable<Int32?>),
            typeof (IEnumerable<Int64?>),
            typeof (IEnumerable<UInt16?>),
            typeof (IEnumerable<UInt32?>),
            typeof (IEnumerable<UInt64?>),
            typeof (IEnumerable<Byte?>),
            typeof (IEnumerable<SByte?>),
            typeof (IEnumerable<decimal?>),
            typeof (IEnumerable<float?>),
            typeof (IEnumerable<double?>),
            typeof (IEnumerable<DateTime?>),
            typeof (IEnumerable<Guid?>)
        };

        #endregion

        /// <summary>
        /// Gets or sets the supported types.
        /// </summary>
        /// <value>
        /// The supported types.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public override IEnumerable<Type> SupportedTypes => _supportedTypes.Select(x => x);

        /// <summary>
        /// Checks if type is supported by query model binder implementation.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        ///   <c>True</c> if type is supported, otherwise
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool DoesSupportType(Type type)
        {
            if (_supportedTypes.Contains(type))
                return true;

            Type collectionElementType;
            if (type.IsSupportedCollection(out collectionElementType))
            {
                Type temp;
                if (type.IsNullable(out temp))
                {
                    collectionElementType = temp;
                }
                return collectionElementType.GetTypeInfo().IsEnum;
            }
            return false;
        }

        /// <summary>
        /// Parses query parameter
        /// </summary>
        /// <param name="request">HTTP request from which to retrieve value(s).</param>
        /// <param name="actionCtx">Action context of the parameter.</param>
        /// <param name="parameter">The parameter to parse.</param>
        /// <returns>
        /// object, Value of the parameter
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override object ParseParameterValue(HttpRequest request, ActionContext actionCtx, ActionParameter parameter)
        {
            CollectionsQueryModelParameterDetails details = GetDetailsForActionParameter(parameter);
            string key = null;
            IEnumerable<KeyValuePair<string, StringValues>> source = null;
            if (parameter.ParameterSource == ParameterSources.Query)
            {
                source = request.Query;
            }
            else if (parameter.ParameterSource == ParameterSources.Header)
            {
                source = request.Headers;
            }

            if (source != null)
            {
                key = source.FirstOrDefault(x => x.Key.ToLower() == parameter.Name).Key;
            }

            string[] values = new string[0];
            if (key != null)
            {
                values = source.FirstOrDefault(x => x.Key == key).Value;
                if (values == null) values = new string[0];
            }

            if (details.IsArray || details.IsIEnumerable)
            {
                Array a = Array.CreateInstance(details.OriginalCollectionElementType, values.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    a.SetValue(ParseSingleQueryValue(
                        values[i],
                        details.CollectionElementType,
                        details.IsCollectionElementTypeNullable,
                        parameter.Name, 
                        new Lazy<string>(() => parameter.ParentActionContext.ToString()), request.HttpContext),
                        i
                        );
                }
                return a;
            }
            if (details.IsList)
            {
                IList parsedValues = Activator.CreateInstance(parameter.Type) as IList;
                foreach (var value in values)
                {
                    parsedValues.Add(ParseSingleQueryValue(
                        value, 
                        details.CollectionElementType, 
                        details.IsCollectionElementTypeNullable, 
                        parameter.Name, 
                        new Lazy<string>(() => parameter.ParentActionContext.ToString()),
                        request.HttpContext));
                }
                return parsedValues;
            }
            
            throw new Exception($"Parameter {parameter} in action {actionCtx} is not array, list or IEnumerable.");
        }

        private CollectionsQueryModelParameterDetails GetDetailsForActionParameter(ActionParameter actionParam)
        {
            string key = nameof(CollectionsQueryModelParameterDetails);
            var value = actionParam.GetAdditionalDataOrDefault<CollectionsQueryModelParameterDetails>(key);
            if (value == null)
            {
                value = new CollectionsQueryModelParameterDetails(actionParam.Type);
                actionParam.SetAdditionalData(key, value);
            }
            return value;
        }

        private class CollectionsQueryModelParameterDetails
        {
            public bool IsArray { get; private set; }
            public bool IsList { get; private set; }
            public bool IsIEnumerable { get; private set; }
            public bool IsCollectionElementTypeNullable { get; private set; }
            public Type CollectionElementType { get; private set; }
            public Type OriginalCollectionElementType { get; private set; }

            public CollectionsQueryModelParameterDetails(Type parameterType)
            {
                IsArray = parameterType.IsArray;
                var info = parameterType.GetTypeInfo();

                if (IsArray)
                {
                    CollectionElementType = parameterType.GetElementType();
                }
                else
                {
                    IsList = info.IsGenericType && info.GetGenericTypeDefinition() == typeof(List<>);
                    if (IsList)
                    {
                        CollectionElementType = info.GenericTypeArguments.Single();
                    }
                }
                if (!IsArray && !IsList)
                {
                    IsIEnumerable = info.IsGenericType && info.GetGenericTypeDefinition() == typeof(IEnumerable<>);
                    if (IsIEnumerable)
                    {
                        CollectionElementType = info.GenericTypeArguments.Single();
                    }
                }

                if (IsArray || IsList || IsIEnumerable)
                {
                    OriginalCollectionElementType = CollectionElementType;
                    Type nullableArgument;
                    if (CollectionElementType.GetTypeInfo().IsNullable(out nullableArgument))
                    {
                        IsCollectionElementTypeNullable = true;
                        CollectionElementType = nullableArgument;
                    }
                }
            }
        }
    }
}
