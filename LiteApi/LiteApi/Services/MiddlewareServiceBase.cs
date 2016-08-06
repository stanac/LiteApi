using LiteApi.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Services
{
    public abstract class MiddlewareServiceBase
    {
        protected T Get<T>()
        {
            throw new NotImplementedException();
        }
    }
}
