using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LiteApi.Contracts.Abstractions
{
    public interface IActionInvoker
    {
        Task Invoke(HttpContext httpCtx, ActionContext actionCtx);
    }
}
