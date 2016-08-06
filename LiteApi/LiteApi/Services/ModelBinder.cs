using LiteApi.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace LiteApi.Services
{
    public class ModelBinder : IModelBinder
    {
        public object[] GetParameterValues(HttpRequest request, ActionContext actionCtx)
        {
            throw new NotImplementedException();
        }
    }
}
