using LiteApi.Contracts.Models;

namespace LiteApi.Contracts.Abstractions
{
    public interface IParametersDiscoverer
    {
        ActionParameter[] GetParameters(ActionContext actionCtx);
    }
}
