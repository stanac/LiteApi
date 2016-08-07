using LiteApi.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace LiteApi.Services
{
    public class ActionInvoker : IActionInvoker
    {
        private readonly IControllerBuilder _controllerBuilder;
        private readonly IModelBinder _modelBinder;

        public ActionInvoker(IControllerBuilder controllerBuilder, IModelBinder modelBinder)
        {
            if (controllerBuilder == null) throw new ArgumentNullException(nameof(controllerBuilder));
            if (modelBinder == null) throw new ArgumentNullException(nameof(modelBinder));
            _controllerBuilder = controllerBuilder;
            _modelBinder = modelBinder;
        }

        public Task Invoke(HttpContext httpCtx, ActionContext action)
        {
            LiteController ctrl = _controllerBuilder.Build(action.ParentController);
            object[] paramValues = _modelBinder.GetParameterValues(httpCtx.Request, action);

            if (action.Method.ReturnType == typeof(Task))
            {
                return action.Method.Invoke(ctrl, paramValues) as Task;
            }

            return Task.Run(() => action.Method.Invoke(ctrl, paramValues));
        }
    }
}
