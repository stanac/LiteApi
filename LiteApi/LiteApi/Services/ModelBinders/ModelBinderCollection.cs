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
        private List<IQueryModelBinder> _queryBinders = new List<IQueryModelBinder>();
        private List<IBodyModelBinder> _bodyBinders = new List<IBodyModelBinder>();
        private Type[] _supportedTypesFromUrl;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBinderCollection"/> class.
        /// </summary>
        /// <param name="jsonSerializer">The JSON serialize.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="optionsRetriever">Options retriever.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ModelBinderCollection(IJsonSerializer jsonSerializer, IServiceProvider serviceProvider, ILiteApiOptionsAccessor optionsRetriever)
        {
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

            _queryBinders.Add(new BasicQueryModelBinder(optionsRetriever));
            _queryBinders.Add(new CollectionsQueryModelBinder(optionsRetriever));
            _queryBinders.Add(new DictionaryQueryModelBinder(optionsRetriever));

            _bodyBinders.Add(new FormFileBodyBinder());
        }

        /// <summary>
        /// Adds an additional query model binder.
        /// </summary>
        /// <param name="queryModelBinder">The query model binder to add.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public void AddAdditionalQueryModelBinder(IQueryModelBinder queryModelBinder)
        {
            if (queryModelBinder == null) throw new ArgumentNullException(nameof(queryModelBinder));
            _queryBinders.Add(queryModelBinder);
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
            if (source == ParameterSources.Query || source == ParameterSources.Header)
            {
                foreach (var b in _queryBinders)
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
            throw new Exception("Error in ModelBinderCollection.DoesSupportType Parameter source needs to be set to Body, Query or Header");
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
                if (param.ParameterSource == ParameterSources.Query || param.ParameterSource == ParameterSources.Header)
                {
                    var binder = _queryBinders.FirstOrDefault(x => x.DoesSupportType(param.Type));
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
                    IBodyModelBinder bodyBinder;
                    if ((bodyBinder = _bodyBinders.FirstOrDefault(x => x.CanHandleType(param.Type))) != null)
                    {
                        args.Add(bodyBinder.CreateParameter(request));
                        continue;
                    }
                    using (TextReader reader = new StreamReader(request.Body))
                    {
                        string json = reader.ReadToEnd();
                        args.Add(_jsonSerializer.Deserialize(json, param.Type));
                    }
                    request.Body.Dispose();
                }
                else if (param.ParameterSource == ParameterSources.Service)
                {
                    args.Add(_serviceProvider.GetService(param.Type));
                }
                else if (param.ParameterSource == ParameterSources.RouteSegment)
                {
                    args.Add(RouteSegmentModelBinder.GetParameterValue(actionCtx, param, request));
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
                _supportedTypesFromUrl = _queryBinders
                    .SelectMany(x => x.SupportedTypes)
                    .Distinct()
                    .ToArray();
            }
            return _supportedTypesFromUrl.Select(x => x);
        }
    }
}
