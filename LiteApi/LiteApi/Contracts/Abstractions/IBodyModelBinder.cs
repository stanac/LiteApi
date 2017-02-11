using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Contract for model binders that can handle types from body
    /// </summary>
    public interface IBodyModelBinder
    {
        /// <summary>
        /// Determines whether this model binder can handle specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>True, if can</returns>
        bool CanHandleType(Type type);

        /// <summary>
        /// Creates the parameter.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Instance of the parameter if parameters is present in the body</returns>
        object CreateParameter(HttpRequest request);
    }
}
