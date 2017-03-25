using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiteApi.Services.ModelBinders
{
    /// <summary>
    /// Query model binder for IDictionary and Dictionary
    /// </summary>
    /// <seealso cref="LiteApi.Services.ModelBinders.BasicQueryModelBinder" />
    internal class DictionaryQueryModelBinder : BasicQueryModelBinder
    {
        /// <summary>
        /// Gets or the supported types, supports dictionaries, use <see cref="DoesSupportType"/>
        /// </summary>
        /// <value>
        /// The supported types.
        /// </value>
        /// <exception cref="System.NotImplementedException"></exception>
        public override IEnumerable<Type> SupportedTypes
        {
            get
            {
                yield return typeof(Dictionary<,>);
                yield return typeof(IDictionary<,>);
            }
        }

        /// <summary>
        /// Checks if type is supported by query model binder implementation.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        ///   <c>True</c> if type is supported, otherwise <c>False</c>
        /// </returns>
        public override bool DoesSupportType(Type type)
        {
            TypeInfo info = type.GetTypeInfo();
            bool isDictionary = info.IsGenericType && SupportedTypes.Contains(type.GetGenericTypeDefinition());
            if (isDictionary)
            {
                Type[] genericArguments = info.GetGenericArguments();
                Type[] baseTypes = base.SupportedTypes.ToArray();
                return genericArguments.All(x => baseTypes.Contains(x));
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
            // todo: support for header parameters
            char[] split = { '.' };
            var details = GetActionParameterDictionaryDetails(parameter);
            var queryKeys = request.Query.Where(x => x.Key.StartsWith(parameter.Name, StringComparison.OrdinalIgnoreCase) && x.Key.Contains("."));

            Func<string, string> getDictionaryKey = key => string.Join(".", key.Split(split).Skip(1));
            IDictionary dictionary = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(details.OriginalKeyType, details.OriginalValueType)) as IDictionary;

            foreach (var queryKeyValues in queryKeys)
            {
                object key = getDictionaryKey(queryKeyValues.Key);
                key = ParseSingleQueryValue(key as string, details.KeyType, details.IsKeyTypeNullable, parameter.Name, new Lazy<string>(() => parameter.ParentActionContext.ToString()));
                object value = queryKeyValues.Value.Last();
                value = ParseSingleQueryValue(value as string, details.ValueType, details.IsValueTypeNullable, parameter.Name, new Lazy<string>(() => parameter.ParentActionContext.ToString()));
                dictionary.Add(key, value);
            }
            return dictionary;
        }

        private static DictionaryQueryModelDetails GetActionParameterDictionaryDetails(ActionParameter actionParam)
        {
            var key = nameof(DictionaryQueryModelDetails);
            var details = actionParam.GetAdditionalDataOrDefault<DictionaryQueryModelDetails>(key);
            if (details == null)
            {
                details = new DictionaryQueryModelDetails(actionParam.Type);
                actionParam.SetAdditionalData(key, details);
            }
            return details;
        }

        private class DictionaryQueryModelDetails
        {
            public Type OriginalKeyType { get; private set; }
            public Type OriginalValueType { get; private set; }
            public Type KeyType { get; private set; }
            public bool IsKeyTypeNullable { get; private set; }
            public Type ValueType { get; private set; }
            public bool IsValueTypeNullable { get; private set; }

            public DictionaryQueryModelDetails(Type dictionaryType)
            {
                var args = dictionaryType.GetTypeInfo().GenericTypeArguments;
                KeyType = args[0];
                ValueType = args[1];
                OriginalKeyType = KeyType;
                OriginalValueType = ValueType;

                Type nullableArgument;
                if (KeyType.GetTypeInfo().IsNullable(out nullableArgument))
                {
                    KeyType = nullableArgument;
                    IsKeyTypeNullable = true;
                }

                if (ValueType.GetTypeInfo().IsNullable(out nullableArgument))
                {
                    ValueType = nullableArgument;
                    IsValueTypeNullable = true;
                }
            }
        }
    }
}
