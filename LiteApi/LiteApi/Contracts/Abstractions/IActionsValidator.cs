using LiteApi.Contracts.Models;
using System.Collections.Generic;

namespace LiteApi.Contracts.Abstractions
{
    public interface IActionsValidator
    {
        IEnumerable<string> GetValidationErrors(ActionContext[] actionCtxs);
    }
}
