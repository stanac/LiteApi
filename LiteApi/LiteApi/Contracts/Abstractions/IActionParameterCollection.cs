using LiteApi.Contracts.Models;
using System.Collections.Generic;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Collection of action parameters
    /// </summary>
    public interface IActionParameterCollection
    {
        /// <summary>
        /// Gets the action parameters.
        /// </summary>
        /// <value>
        /// The action parameters.
        /// </value>
        IEnumerable<ActionParameter> Parameters { get; }
    }
}
