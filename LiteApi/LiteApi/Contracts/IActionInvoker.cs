using System.Threading.Tasks;

namespace LiteApi.Contracts
{
    public interface IActionInvoker
    {
        Task Invoke(ActionContext actionCtx, ActionParameter[] actionParameters);
    }
}
