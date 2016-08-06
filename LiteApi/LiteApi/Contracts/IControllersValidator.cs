using System.Collections;
using System.Collections.Generic;

namespace LiteApi.Contracts
{
    public interface IControllersValidator
    {
        IEnumerable<string> GetValidationErrors(ControllerContext[] controllerCtxs);
    }
}
