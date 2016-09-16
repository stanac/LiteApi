using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi.Services.ModelBinders
{
    /// <summary>
    /// Class for resolving parameter values for given <see cref="ActionContext"/>
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IModelBinder" />
    internal class BasicQueryModelBinder : IQueryModelBinder
    {
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

        #endregion

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
            var paramName = parameter.Name.ToLower();
            if (request.Query.Keys.All(x => x != paramName))
            {
                if (parameter.HasDefaultValue)
                {
                    return parameter.DefaultValue;
                }
                throw new Exception($"Parameter {parameter.Name} from query does not have default value and query does not contain value.");
            }
            var value = request.Query.Where(x => x.Key == paramName).Last().Value.Last();
            return ParseSingleQueryValue(value, parameter.Type, parameter.IsNullable, parameter.Name);
        }

        /// <summary>
        /// Parses the single query value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="type">The type.</param>
        /// <param name="isNullable">if set to <c>true</c> parameter is nullable, as in int? or Nullable&lt;bool&gt;.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected object ParseSingleQueryValue(string value, Type type, bool isNullable, string parameterName)
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

            throw new ArgumentOutOfRangeException();
        }

    }
}
