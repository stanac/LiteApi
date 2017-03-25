using System;
using System.Linq;
using LiteApi.Contracts.Abstractions;
using System.Reflection;
using System.Collections;

namespace LiteApi.Contracts.Models
{
    /// <summary>
    /// Metadata for action parameter that is received by the API.
    /// </summary>
    public class ActionParameter : AdditionalData
    {
        private Type _type;
        private Type _originalType;
        private Type _collectionElementType;
        private bool _isTypeNullable;
        private bool _isTypeCollection;
        private bool _isCollectionElementTypeNullable;
        private readonly IModelBinder _modelBinders;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionParameter"/> class.
        /// </summary>
        /// <param name="parentActionCtx">The parent action context.</param>
        /// <param name="modelBinders">Model binders.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ActionParameter(ActionContext parentActionCtx, IModelBinder modelBinders)
        {
            if (parentActionCtx == null) throw new ArgumentNullException(nameof(parentActionCtx));
            if (modelBinders == null) throw new ArgumentNullException(nameof(modelBinders));
            ParentActionContext = parentActionCtx;
            _modelBinders = modelBinders;
        }

        /// <summary>
        /// Gets or sets the parent action context.
        /// </summary>
        /// <value>
        /// The parent action context.
        /// </value>
        public ActionContext ParentActionContext { get; set; }

        internal bool IsTypeSupportedFromRoute()
        {
            if (IsNullable) return false;
            return !IsComplex;
        }

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
                _originalType = value;
                _type = value;
                TypeInfo info = value.GetTypeInfo();
                Type nullableArgument;
                if (_isTypeNullable = info.IsNullable(out nullableArgument))
                {
                    _type = nullableArgument;
                }
                else if (value.IsArray)
                {
                    _isTypeCollection = true;
                    _collectionElementType = value.GetElementType();
                    Type temp;
                    if (_collectionElementType.IsNullable(out temp))
                    {
                        _isCollectionElementTypeNullable = true;
                        _collectionElementType = temp;
                    }
                }
                else if (info.IsGenericType && typeof(IEnumerable).IsAssignableFrom(value))
                {
                    _isTypeCollection = true;
                    _collectionElementType = info.GetGenericArguments().First();
                    Type temp;
                    if (_collectionElementType.IsNullable(out temp))
                    {
                        _isCollectionElementTypeNullable = true;
                        _collectionElementType = temp;
                    }
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
        /// Gets a value indicating whether this instance is collection element type nullable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is collection element type nullable; otherwise, <c>false</c>.
        /// </value>
        public bool IsCollectionElementTypeNullable => _isCollectionElementTypeNullable;

        /// <summary>
        /// Gets a value indicating whether this action parameter is collection.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this action parameter is collection; otherwise, <c>false</c>.
        /// </value>
        public bool IsCollection => _isTypeCollection;

        /// <summary>
        /// Gets the type of the collection element.
        /// </summary>
        /// <value>
        /// The type of the collection element.
        /// </value>
        public Type CollectionElementType => _collectionElementType;

        /// <summary>
        /// Gets a value indicating whether the parameter is complex).
        /// </summary>
        /// <value>
        /// <c>true</c> if the parameter is complex; otherwise, <c>false</c>.
        /// </value>
        public bool IsComplex
        {
            get
            {
                // todo: remove this from model class
                return !_modelBinders.DoesSupportType(Type, ParameterSources.Query);
            }
        }

        /// <summary>
        /// Gets or sets the overriden parameter name (Used only for header parameters for now).
        /// </summary>
        /// <value>
        /// The overriden parameter name.
        /// </value>
        public string OverridenName { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return $"{_originalType.GetFriendlyName()} {Name}";
        }

    }
}
