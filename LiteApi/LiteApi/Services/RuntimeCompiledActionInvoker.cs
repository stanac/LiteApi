using LiteApi.Contracts.Abstractions;
using System.Threading.Tasks;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace LiteApi.Services
{
    public class RuntimeCompiledActionInvoker : IActionInvoker
    {
        public static Func<IJsonSerializer> GetJsonSerializer { get; set; } = () => LiteApiMiddleware.Options.JsonSerializer;
        private readonly CompiledActionInvokerCache _cache = new CompiledActionInvokerCache();
        private readonly IControllerBuilder _controllerBuilder;
        private readonly IModelBinder _modelBinder;

        public RuntimeCompiledActionInvoker(IControllerBuilder controllerBuilder, IModelBinder modelBinder)
        {
            if (controllerBuilder == null) throw new ArgumentNullException(nameof(controllerBuilder));
            if (modelBinder == null) throw new ArgumentNullException(nameof(modelBinder));
            _controllerBuilder = controllerBuilder;
            _modelBinder = modelBinder;
        }

        public async Task Invoke(HttpContext httpCtx, ActionContext actionCtx)
        {
            ApiFilterRunResult filterResult = await ActionInvoker.RunFiltersAndCheckIfShouldContinue(httpCtx, actionCtx);

            if (!filterResult.ShouldContinue)
            {
                if (filterResult.SetResponseCode.HasValue)
                {
                    httpCtx.Response.StatusCode = filterResult.SetResponseCode.Value;
                }
                return;
            }

            object[] paramValues = _modelBinder.GetParameterValues(httpCtx.Request, actionCtx);
            var proxy = _cache.GetProxy(actionCtx.ActionGuid);
            LiteController ctrl = _controllerBuilder.Build(actionCtx.ParentController, httpCtx);

            object result = null;

            if (proxy.IsVoid)
            {
                if (proxy.IsAsync)
                {
                    await proxy.InvokeVoidTask(ctrl, paramValues);
                }
                else
                {
                    proxy.InvokeVoid(ctrl, paramValues);
                }
            }
            else
            {
                if (proxy.IsAsync)
                {
                    var task = proxy.InvokeNotVoidTask(ctrl, paramValues);
                    result = await task;
                }
                else
                {
                    result = proxy.InvokeNotVoid(ctrl, paramValues);
                }
            }

            int statusCode = 405; // method not allowed
            switch (httpCtx.Request.Method)
            {
                case "GET": statusCode = 200; break;
                case "POST": statusCode = 201; break;
                case "PUT": statusCode = 201; break;
                case "DELETE": statusCode = 204; break;

            }
            httpCtx.Response.StatusCode = statusCode;
            if (!proxy.IsVoid)
            {
                httpCtx.Response.ContentType = "application/json";
                await httpCtx.Response.WriteAsync(GetJsonSerializer().Serialize(result));
            }
        }
    }
}
