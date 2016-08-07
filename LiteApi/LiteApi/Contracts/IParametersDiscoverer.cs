using System.Reflection;

namespace LiteApi.Contracts
{
    public interface IParametersDiscoverer
    {
        ActionParameter[] GetParameters(ActionContext actionCtx);
    }
}
