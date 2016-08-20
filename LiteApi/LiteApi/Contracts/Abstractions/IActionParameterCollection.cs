using LiteApi.Contracts.Models;
using System.Collections.Generic;

namespace LiteApi.Contracts.Abstractions
{
    public interface IActionParameterCollection
    {
        IEnumerable<ActionParameter> Parameters { get; }
    }
}
