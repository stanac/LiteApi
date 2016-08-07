using System.Collections.Generic;

namespace LiteApi.Contracts
{
    public interface IParametersValidator
    {
        IEnumerable<string> GetParametersErrors(ActionContext actionCtx);
    }
}
