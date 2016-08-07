using LiteApi.Contracts.Models;
using System.Collections;
using System.Collections.Generic;

namespace LiteApi.Contracts.Abstractions
{
    public interface IControllersValidator
    {
        IEnumerable<string> GetValidationErrors(ControllerContext[] controllerCtxs);
    }
}
