using System;
using System.Collections.Generic;
using System.Linq;
using LiteApi.Contracts.Abstractions;
using System.Reflection;
using System.Collections;
using LiteApi.Services.ModelBinders;

namespace LiteApi.Contracts.Models
{
    /// <summary>
    /// Metadata for action parameter that is received by the API.
    /// </summary>
    public class ActionParameter : AdditionalData
    {
        internal bool _isTypeNullable;
        internal Type _type;
        private static Type[] _supportedTypesFromUrl;

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
            }
        }
        
        /// <summary>
        /// Gets a value indicating whether this action parameter is nullable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this action parameter is nullable; otherwise, <c>false</c>.
        /// </value>
        public bool IsNullable => _isTypeNullable;

        /// <summary>
        /// Gets a value indicating whether the parameter is complex (not in <see cref="GetSupportedTypesFromUrl"/>).
        /// </summary>
        /// <value>
        /// <c>true</c> if the parameter is complex; otherwise, <c>false</c>.
        /// </value>
        public bool IsComplex => !GetSupportedTypesFromUrl().Contains(Type);

        #region SupportedTypesFromUrl
        
        #endregion
                
        /// <summary>
        /// Gets the supported types.
        /// </summary>
        /// <returns>Collection of supported <see cref="Type"/> from URL</returns>
        public static IEnumerable<Type> GetSupportedTypesFromUrl()
        {
            if (_supportedTypesFromUrl == null)
            {
                _supportedTypesFromUrl = new ModelBinderCollection(new Services.JsonSerializer()).GetSupportedTypesFromUrl().ToArray();
            }
            return _supportedTypesFromUrl.Select(x => x);
        }
        
    }
}
