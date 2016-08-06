using Microsoft.AspNetCore.Http;

namespace LiteApi.Contracts
{
    public interface IPathResolver
    {
        ActionContext ResolvePath(HttpRequest request);
    }
}
