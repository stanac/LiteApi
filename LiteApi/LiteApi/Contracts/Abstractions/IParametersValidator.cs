using LiteApi.Contracts.Models;
using System.Collections.Generic;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Contract for validating action parameters.
    /// </summary>
    public interface IParametersValidator
    {
        /// <summary>
        /// Gets the parameters errors.
        /// </summary>
        /// <param name="actionCtx">The action CTX.</param>
        /// <returns>Collection of strings that contains errors, if not empty an exception should be raised.</returns>
        IEnumerable<string> GetParametersErrors(ActionContext actionCtx);
    }
}
