using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace LiteApi.Contracts.Abstractions
{
    public interface IModelBinder
    {
        object[] GetParameterValues(HttpRequest request, ActionContext actionCtx);
    }
}
