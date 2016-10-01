using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LiteApi.Services.ModelBinders
{
    /// <summary>
    /// Collection of all in use model binders, this should be the entry point for all other model binders
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IModelBinder" />
    public class ModelBinderCollection : IModelBinder
    {
        private List<IQueryModelBinder> _binders = new List<IQueryModelBinder>();
        private readonly IJsonSerializer _jsonSerializer;
        private Type[] _supportedTypesFromUrl;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBinderCollection"/> class.
        /// </summary>
        /// <param name="jsonSerializer">The json serialize.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ModelBinderCollection(IJsonSerializer jsonSerializer)
        {
            if (jsonSerializer == null) throw new ArgumentNullException(nameof(jsonSerializer));
            _jsonSerializer = jsonSerializer;
            
            _binders.Add(new BasicQueryModelBinder());
            _binders.Add(new CollectionsQueryModelBinder());
            _binders.Add(new DictionaryQueryModelBinder());
        }

        /// <summary>
        /// Adds an additional query model binder.
        /// </summary>
        /// <param name="queryModelBinder">The query model binder to add.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void AddAdditionalQueryModelBinder(IQueryModelBinder queryModelBinder)
        {
            if (queryModelBinder == null) throw new ArgumentNullException(nameof(queryModelBinder));
            _binders.Add(queryModelBinder);
        }
        
        /// <summary>
        /// Checks if type is supported by model binder instance.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="source"></param>
        /// <returns>
        ///   <c>true</c> is parameter is supported, otherwise <c>false</c>
        /// </returns>
        public bool DoesSupportType(Type type, ParameterSources source)
        {
            if (source == ParameterSources.Query)
            {
                foreach (var b in _binders)
                {
                    if (b.DoesSupportType(type))
                    {
                        return true;
                    }
                }
                return false;
            }
            if (source == ParameterSources.Body)
            {
                return true;
            }
            throw new Exception("Error in ModelBinderCollection.DoesSupportType Parameter source needs to be set to Body or Query");
        }

        /// <summary>
        /// Gets the parameter values from the HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request.</param>
        /// <param name="actionCtx">The action context which should be invoked.</param>
        /// <returns>
        /// Collection of parameters to be passed to action when being invoked
        /// </returns>
        /// <exception cref="System.InvalidOperationException">If parameter is not provided from query</exception>
        /// <exception cref="System.ArgumentException"></exception>
        public object[] GetParameterValues(HttpRequest request, ActionContext actionCtx)
        {
            object[] values = new object[actionCtx.Parameters.Length];
            List<object> args = new List<object>();
            foreach (var param in actionCtx.Parameters)
            {
                if (param.ParameterSource == ParameterSources.Query)
                {
                    var binder = _binders.First(x => x.DoesSupportType(param.Type));
                    if (binder != null)
                    {
                        args.Add(binder.ParseParameterValue(request, actionCtx, param));
                    }
                    else
                    {
                        throw new Exception($"No model binder supports type: {param.Type}");
                    }
                }
                else if (param.ParameterSource == ParameterSources.Body)
                {
                    using (TextReader reader = new StreamReader(request.Body))
                    {
                        string json = reader.ReadToEnd();
                        args.Add(_jsonSerializer.Deserialize(json, param.Type));
                    }
                    request.Body.Dispose();
                }
                else if (param.ParameterSource == ParameterSources.RouteSegment)
                {
                    args.Add(RouteSegmentQueryModelBinder.GetParameterValue(actionCtx, param, request));
                }
                else
                {
                    throw new ArgumentException(
                        $"Parameter {param.Name} in controller {actionCtx.ParentController.RouteAndName} in action {actionCtx.Name} "
                        + "has unknown source (body or URL). " + AttributeConventions.ErrorResolutionSuggestion);
                }
            }
            return args.ToArray();
        }

        /// <summary>
        /// Gets the supported types from URL.
        /// </summary>
        /// <returns>Collection of supported types</returns>
        public IEnumerable<Type> GetSupportedTypesFromUrl()
        {
            if (_supportedTypesFromUrl == null)
            {
                _supportedTypesFromUrl = _binders
                    .SelectMany(x => x.SupportedTypes)
                    .Distinct()
                    .ToArray();
            }
            return _supportedTypesFromUrl.Select(x => x);
        }
    }
}
