using Microsoft.AspNetCore.Http;

namespace LiteApi.Contracts.Abstractions
{
    interface IParameterMatcher
    {
        int GetMatchWeight(IActionParameterCollection parameters, HttpRequest request);
    }
}
