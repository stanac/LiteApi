using LiteApi.Contracts.Models;
using System.Collections.Generic;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// When implemented validates actions before first request is received.
    /// </summary>
    public interface IActionsValidator
    {
        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <param name="actionCtxs">The action context.</param>
        /// <returns>Collection of strings that contains errors, if not empty an exception should be raised.</returns>
        IEnumerable<string> GetValidationErrors(ActionContext[] actionCtxs);
    }
}
