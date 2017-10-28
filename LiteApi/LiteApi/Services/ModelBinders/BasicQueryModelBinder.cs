using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi.Services.ModelBinders
{
    /// <summary>
    /// Class for resolving parameter values for given <see cref="ActionContext"/>
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IModelBinder" />
    public class BasicQueryModelBinder : IQueryModelBinder
    {
        private readonly ILiteApiOptionsRetriever _optionsRetriever;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicQueryModelBinder"/> class.
        /// </summary>
        /// <param name="liteApiOptionsRetriever">The lite API options retriever.</param>
        public BasicQueryModelBinder(ILiteApiOptionsRetriever liteApiOptionsRetriever)
        {
            _optionsRetriever = liteApiOptionsRetriever ?? throw new ArgumentNullException(nameof(liteApiOptionsRetriever));
        }

        #region _supportedTypes

        private static readonly Type[] _supportedTypes =
        {
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
            typeof (DateTimeOffset),
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
            typeof (DateTimeOffset?),
            typeof (DateTime?),
            typeof (Guid?)
        };

        #endregion

        /// <summary>
        /// Gets or sets the supported types.
        /// </summary>
        /// <value>
        /// The supported types.
        /// </value>
        public virtual IEnumerable<Type> SupportedTypes => _supportedTypes.Select(x => x);
       
        /// <summary>
        /// Checks if type is supported by query model binder implementation.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        ///   <c>True</c> if type is supported, otherwise
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual bool DoesSupportType(Type type) => _supportedTypes.Contains(type);

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
        public virtual object ParseParameterValue(HttpRequest request, ActionContext actionCtx, ActionParameter parameter)
        {
            // todo: refactor so we don't check all keys
            string value = null;
            var paramName = parameter.Name;
            if (!string.IsNullOrWhiteSpace(parameter.OverridenName)) paramName = parameter.OverridenName;

            IEnumerable<KeyValuePair<string, StringValues>> source = null;
            if (parameter.ParameterSource == ParameterSources.Query) source = request.Query;
            else source = request.Headers;
            
            var keyValue = source.LastOrDefault(x => paramName.Equals(x.Key, StringComparison.OrdinalIgnoreCase));

            if (keyValue.Key != null)
            {
                value = keyValue.Value.LastOrDefault();
            }

            if (keyValue.Key == null)
            {
                if (parameter.HasDefaultValue) return parameter.DefaultValue;
                string message =
                    $"Parameter '{parameter.Name}' from {parameter.ParameterSource.ToString().ToLower()} " +
                    $"(action: '{parameter.ParentActionContext}') does not have default value and " +
                    $"{parameter.ParameterSource.ToString().ToLower()} does not contain value.";
                throw new Exception(message);
            }

            if (parameter.HasDefaultValue && parameter.Type != typeof(string) && string.IsNullOrEmpty(value)) return parameter.DefaultValue;
            
            return ParseSingleQueryValue(
                value, 
                parameter.Type, 
                parameter.IsNullable, 
                parameter.Name, 
                new Lazy<string>(() => parameter.ParentActionContext.ToString()),
                request.HttpContext);
        }

        /// <summary>
        /// Parses the single query value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <param name="isNullable">if set to <c>true</c> parameter is nullable, as in int? or Nullable&lt;bool&gt;.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="actionNameRetriever">Resolves name of the action in lazy manner</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        public static object ParseSingleQueryValue(string value, Type type, bool isNullable, string parameterName, Lazy<string> actionNameRetriever, HttpContext httpCtx)
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
                throw new ArgumentException($"Value is not provided for parameter: '{parameterName}' in action '{actionNameRetriever.Value}'");
            }
            // todo: check if using switch with Type.GUID.ToString() would be faster
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
            if (type == typeof(DateTime)) return ParseDateTime(value, httpCtx);
            if (type == typeof(DateTimeOffset)) return DateTimeOffset.Parse(value);
            if (type == typeof(Guid)) return Guid.Parse(value);

            throw new ArgumentOutOfRangeException();
        }
        
        private static DateTime ParseDateTime(string value, HttpContext httpCtx)
        {
            var action = httpCtx.GetActionContext();
            var options = httpCtx.GetLiteApiOptions();
            string format = action.DateTimeParsingFormat;
            if (format == null) format = action.ParentController.DateTimeParsingFormat;
            if (format == null) format = options.GlobalDateTimeParsingFormat;

            if (format == null) return DateTime.Parse(value);

            var formatInfo = options.DateTimeParsingFormatProviderFactory(httpCtx);
            return DateTime.ParseExact(value, format, formatInfo);
        }
    }
}
