using Microsoft.AspNetCore.Http;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Contract that can retrieve action match wight, it's used for action overloading.
    /// </summary>
    interface IParameterMatcher
    {
        /// <summary>
        /// Gets the match weight.
        /// </summary>
        /// <param name="parameters">The action parameters.</param>
        /// <param name="request">The HTTP request.</param>
        /// <returns>Weight of the match</returns>
        int GetMatchWeight(IActionParameterCollection parameters, HttpRequest request);
    }
}
