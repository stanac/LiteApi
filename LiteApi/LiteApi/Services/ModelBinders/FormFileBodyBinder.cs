using LiteApi.Contracts.Abstractions;
using System;
using Microsoft.AspNetCore.Http;

namespace LiteApi.Services.ModelBinders
{
    /// <summary>
    /// Model binder for files submitted using form
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IBodyModelBinder" />
    internal class FormFileBodyBinder : IBodyModelBinder
    {
        /// <summary>
        /// Determines whether this model binder can handle specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        /// True if can
        /// </returns>
        public bool CanHandleType(Type type)
        {
            return type == typeof(FormFileCollection);
        }

        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>
        /// Instance of the parameter if parameters is present in the body
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object CreateParameter(HttpRequest request)
        {
            return new FormFileCollection(request);
        }
    }
}
