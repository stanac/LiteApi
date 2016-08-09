using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace LiteApi.Services
{
    public class ActionInvoker : IActionInvoker
    {
        public static Func<IJsonSerializer> GetJsonSerializer { get; set; } = () => LiteApiMiddleware.Options.JsonSerializer;
        private readonly IControllerBuilder _controllerBuilder;
        private readonly IModelBinder _modelBinder;

        public ActionInvoker(IControllerBuilder controllerBuilder, IModelBinder modelBinder)
        {
            if (controllerBuilder == null) throw new ArgumentNullException(nameof(controllerBuilder));
            if (modelBinder == null) throw new ArgumentNullException(nameof(modelBinder));
            _controllerBuilder = controllerBuilder;
            _modelBinder = modelBinder;
        }

        public async Task Invoke(HttpContext httpCtx, ActionContext action)
        {
            ApiFilterRunResult filterResult = await RunFiltersAndCheckIfShouldContinue(httpCtx, action);

            if (!filterResult.ShouldContinue)
            {
                if (filterResult.SetResponseCode.HasValue)
                {
                    httpCtx.Response.StatusCode = filterResult.SetResponseCode.Value;
                }
                return;
            }

            LiteController ctrl = _controllerBuilder.Build(action.ParentController, httpCtx);
            object[] paramValues = _modelBinder.GetParameterValues(httpCtx.Request, action);

            object result = null;
            bool isVoid = true;
            if (action.Method.ReturnType == typeof(void))
            {
                action.Method.Invoke(ctrl, paramValues);
            }
            else if (action.Method.ReturnType == typeof(Task))
            {
                var task = (action.Method.Invoke(ctrl, paramValues) as Task);
                await task;
            }
            else if (action.Method.ReturnType.IsConstructedGenericType && action.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            { 
                isVoid = false;
                var task = (dynamic)(action.Method.Invoke(ctrl, paramValues));
                result = await task;
            }
            else
            {
                isVoid = false;
                result = action.Method.Invoke(ctrl, paramValues);
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
            if (!isVoid)
            {
                httpCtx.Response.ContentType = "application/json";
                await httpCtx.Response.WriteAsync(GetJsonSerializer().Serialize(result));
            }
        }

        internal async Task<ApiFilterRunResult> RunFiltersAndCheckIfShouldContinue(HttpContext httpCtx, ActionContext action)
        {
            if (action.SkipAuth) return new ApiFilterRunResult { ShouldContinue = true };

            ApiFilterRunResult result = await action.ParentController.ValidateFilters(httpCtx);
            if (!result.ShouldContinue)
            {
                return result;
            }

            foreach (var filter in action.Filters)
            {
                result = await filter.ShouldContinueAsync(httpCtx);
                if (!result.ShouldContinue)
                {
                    return result;
                }
            }

            return new ApiFilterRunResult { ShouldContinue = true };
        }
    }
}
