using LiteApi.Contracts.Models;
using System.Collections.Generic;

namespace LiteApi.Contracts.Abstractions
{
    public interface IParametersValidator
    {
        IEnumerable<string> GetParametersErrors(ActionContext actionCtx);
    }
}
