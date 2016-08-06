using LiteApi.Contracts;
using System.Threading.Tasks;

namespace LiteApi.Services
{
    public class ActionInvoker : IActionInvoker
    {
        public Task Invoke(ActionContext action, ActionParameter[] actionParameters)
        {
            //object ctrl = GetController(ctrlType);
            //var parameters = action.ParseParameters(context.Request);
            //object result = action.Method.Invoke(ctrl, parameters);
            //return context.Response.WriteAsync(JsonConvert.SerializeObject(result));
            throw new System.NotImplementedException();
        }
    }
}
