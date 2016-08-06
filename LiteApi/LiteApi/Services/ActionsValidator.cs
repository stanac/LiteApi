using LiteApi.Contracts;
using System;
using System.Collections.Generic;

namespace LiteApi.Services
{
    public class ActionsValidator : IActionsValidator
    {
        public IEnumerable<string> GetValidationErrors(ActionContext[] actionCtxs)
        {
            throw new NotImplementedException();
        }
    }
}
