using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace LiteApi.Contracts
{
    public interface IActionInvoker
    {
        Task Invoke(HttpContext httpCtx, ActionContext actionCtx);
    }
}
