using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;

namespace LiteApi.Tests.Fakes
{
    public class FakeParametersDiscoverer : IParametersDiscoverer
    {
        public ActionParameter[] GetParameters(ActionContext actionCtx) => new ActionParameter[0];
    }
}
