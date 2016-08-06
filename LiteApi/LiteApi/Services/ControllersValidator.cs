using LiteApi.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Services
{
    public class ControllersValidator : IControllersValidator
    {
        public IEnumerable<string> GetValidationErrors(ControllerContext[] controllerCtxs)
        {
            throw new NotImplementedException();
        }
    }
}
