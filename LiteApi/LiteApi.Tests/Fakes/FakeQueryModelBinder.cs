using LiteApi.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace LiteApi.Tests.Fakes
{
    public class FakeQueryModelBinder : IQueryModelBinder
    {
        public IEnumerable<Type> SupportedTypes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool DoesSupportType(Type type)
        {
            throw new NotImplementedException();
        }

        public object ParseParameterValue(HttpRequest request, ActionContext actionCtx, ActionParameter parameter)
        {
            throw new NotImplementedException();
        }
    }
}
