using System.Collections.Generic;

namespace LiteApi.Contracts
{
    public interface IActionsValidator
    {
        IEnumerable<string> GetValidationErrors(ActionContext[] actionCtxs);
    }
}
