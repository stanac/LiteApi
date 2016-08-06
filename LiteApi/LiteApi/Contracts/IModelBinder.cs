using Microsoft.AspNetCore.Http;

namespace LiteApi.Contracts
{
    public interface IModelBinder
    {
        object[] GetParameterValues(HttpRequest request, ActionContext actionCtx);
    }
}
