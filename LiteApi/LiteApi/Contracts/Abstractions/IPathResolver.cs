using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;

namespace LiteApi.Contracts.Abstractions
{
    public interface IPathResolver
    {
        ActionContext ResolvePath(HttpRequest request);
    }
}
