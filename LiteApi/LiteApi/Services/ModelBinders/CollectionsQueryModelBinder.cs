﻿using LiteApi.Contracts.Abstractions;
using System;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;

namespace LiteApi.Services.ModelBinders
{
    /// <summary>
    /// Query model binder that supports lists and arrays
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IQueryModelBinder" />
    internal class CollectionsQueryModelBinder : BasicQueryModelBinder
    {
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
            typeof (List<Guid?>)
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
            => _supportedTypes.Contains(type);

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
            var key = request.Query.Keys.FirstOrDefault(x => x.ToLower() == parameter.Name);
            if (key == null)
            {
                if (parameter.IsNullable)
                {
                    return null;
                }
                throw new Exception($"Non nullable parameter {parameter.Name} is missing from query");
            }
            string[] values = request.Query[key];
            if (values?.Length == 0)
            {
                if (parameter.IsNullable)
                {
                    return null;
                }
                throw new Exception($"Non nullable parameter {parameter.Name} is missing from query");
            }

            if (details.IsArray)
            {
                Array a = Array.CreateInstance(details.OriginalCollectionElementType, values.Length);
                for (int i = 0; i < values.Length; i++)
                {
                    a.SetValue(ParseSingleQueryValue(values[i], details.CollectionElementType, details.IsCollectionElementTypeNullable, parameter.Name), i);
                }
                return a;
            }
            if (details.IsList)
            {
                IList parsedValues = Activator.CreateInstance(parameter.Type) as IList;
                foreach (var value in values)
                {
                    parsedValues.Add(ParseSingleQueryValue(value, details.CollectionElementType, details.IsCollectionElementTypeNullable, parameter.Name));
                }
                return parsedValues;
            }
            throw new Exception($"Parameter is not neither array nor list.");
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
            public bool IsCollectionElementTypeNullable { get; private set; }
            public Type CollectionElementType { get; private set; }
            public Type OriginalCollectionElementType { get; private set; }

            public CollectionsQueryModelParameterDetails(Type parameterType)
            {
                IsArray = parameterType.IsArray;

                if (IsArray)
                {
                    CollectionElementType = parameterType.GetElementType();
                }
                else
                {
                    var info = parameterType.GetTypeInfo();
                    IsList = info.IsGenericType && info.GetGenericTypeDefinition() == typeof(List<>);
                    CollectionElementType = info.GenericTypeArguments.Single();
                }
                if (!IsArray && !IsList)
                {
                    throw new Exception($"Parameter is not neither array nor list.");
                }
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