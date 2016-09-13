using LiteApi.Contracts.Models;
using System.Collections;
using System.Collections.Generic;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// When implemented validates controllers before first request is received.
    /// </summary>
    public interface IControllersValidator
    {
        /// <summary>
        /// Gets the validation errors.
        /// </summary>
        /// <param name="controllerCtxs">The controller context.</param>
        /// <returns>Collection of strings that contains errors, if not empty an exception should be raised.</returns>
        IEnumerable<string> GetValidationErrors(ControllerContext[] controllerCtxs);
    }
}
