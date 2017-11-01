﻿using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Services
{
    /// <summary>
    /// Class that is used for invoking actions
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IActionInvoker" />
    public class ActionInvoker : IActionInvoker
    {
        /// <summary>
        /// Gets or sets JSON serializer.
        /// </summary>
        /// <value>
        /// JSON serializer.
        /// </value>
        private readonly IControllerBuilder _controllerBuilder;
        private readonly IModelBinder _modelBinder;
        private readonly IJsonSerializer _jsonSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionInvoker"/> class.
        /// </summary>
        /// <param name="controllerBuilder">The controller builder.</param>
        /// <param name="modelBinder">The model binder.</param>
        /// <param name="jsonSerializer">The JSON serializer.</param>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public ActionInvoker(IControllerBuilder controllerBuilder, IModelBinder modelBinder, IJsonSerializer jsonSerializer)
        {
            _controllerBuilder = controllerBuilder ?? throw new ArgumentNullException(nameof(controllerBuilder));
            _modelBinder = modelBinder ?? throw new ArgumentNullException(nameof(modelBinder));
            _jsonSerializer = jsonSerializer ?? throw new ArgumentNullException(nameof(jsonSerializer));
        }

        /// <summary>
        /// Invokes the specified <see cref="ActionContext"/>.
        /// </summary>
        /// <param name="httpCtx">The HTTP context, set by the middleware.</param>
        /// <param name="actionCtx">The action context.</param>
        /// <param name="logger">Logger to use, can be null</param>
        /// <returns></returns>
        public virtual async Task Invoke(HttpContext httpCtx, ActionContext actionCtx, ILogger logger = null)
        {
            logger?.LogInformation("Checking filters");
            ApiFilterRunResult filterResult = await RunFiltersAndCheckIfShouldContinue(httpCtx, actionCtx);
            logger?.LogInformation($"Checking filters completed, can invoke: {filterResult.ShouldContinue}");

            if (!filterResult.ShouldContinue)
            {
                int failedStatusCode = 0;
                if (filterResult.SetResponseCode.HasValue)
                {
                    failedStatusCode = filterResult.SetResponseCode.Value;
                }
                else
                {
                    bool isAuthenticated = httpCtx?.User?.Identity?.IsAuthenticated ?? false;
                    if (isAuthenticated)
                    {
                        failedStatusCode = 403;
                    }
                    else
                    {
                        failedStatusCode = 401;
                    }
                }
                httpCtx.Response.StatusCode = failedStatusCode;
                await httpCtx.Response.WriteAsync(filterResult.SetResponseMessage ?? "request rejected");
                logger?.LogInformation($"returning response with status code: {failedStatusCode}");
                return;
            }

            logger?.LogInformation($"Building controller: {actionCtx.ParentController}");
            LiteController ctrl = _controllerBuilder.Build(actionCtx.ParentController, httpCtx);
            object[] paramValues = _modelBinder.GetParameterValues(httpCtx.Request, actionCtx);
            logger?.LogInformation($"Building controller succeeded: {actionCtx.ParentController}");
            logger?.LogInformation($"Invoking action: {actionCtx}");
            object result = null;
            bool isVoid = true;

            var actionExecutionCtx = new ActionExecutingContext(actionCtx.Parameters, paramValues, httpCtx, actionCtx);
            bool shouldContinueCheck = await ctrl.BeforeActionExecution(actionExecutionCtx);
            if (!shouldContinueCheck)
            {
                return;
            }

            if (actionCtx.Method.ReturnType == typeof(void))
            {
                actionCtx.Method.Invoke(ctrl, paramValues);
            }
            else if (actionCtx.Method.ReturnType == typeof(Task))
            {
                var task = (actionCtx.Method.Invoke(ctrl, paramValues) as Task);
                await task;
            }
            else if (actionCtx.Method.ReturnType.IsConstructedGenericType && actionCtx.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            { 
                isVoid = false;
                var task = (dynamic)(actionCtx.Method.Invoke(ctrl, paramValues));
                result = await task;
            }
            else
            {
                isVoid = false;
                result = actionCtx.Method.Invoke(ctrl, paramValues);
            }

            await ctrl.AfterActionExecuted(actionExecutionCtx, result);

            int? overridenResponseCode = httpCtx.GetResponseStatusCode();
            if (overridenResponseCode.HasValue)
            {
                httpCtx.Response.StatusCode = overridenResponseCode.Value;
            }
            else
            {
                int statusCode = 405; // method not allowed
                switch (httpCtx.Request.Method.ToUpper())
                {
                    case "GET": statusCode = 200; break;
                    case "POST": statusCode = 201; break;
                    case "PUT": statusCode = 201; break;
                    case "DELETE": statusCode = 204; break;
                }
                httpCtx.Response.StatusCode = statusCode;
            }

            httpCtx.Response.Headers.Add("X-Powered-By-Middleware", "LiteApi");
            var additionalHeaders = httpCtx.GetResponseHeaders(true);
            if (additionalHeaders != null)
            {
                foreach (var header in additionalHeaders)
                {
                    httpCtx.Response.Headers.Add(header.Key, header.Value);
                }
            }

            if (isVoid)
            {
                logger?.LogInformation("Not serializing result from invoked action, action is void or void task");
            }
            else
            {
                logger?.LogInformation("Serializing result from invoked action");
                if (actionCtx.IsReturningLiteActionResult)
                {
                    await (result as ILiteActionResult).WriteResponse(httpCtx, actionCtx);
                }
                else
                {
                    httpCtx.Response.ContentType = "application/json";
                    await httpCtx.Response.WriteAsync(_jsonSerializer.Serialize(result));
                }
            }
        }

        /// <summary>
        /// Runs the filters and check if should continue.
        /// </summary>
        /// <param name="httpCtx">The HTTP context.</param>
        /// <param name="action">The action context.</param>
        /// <returns>Task to await</returns>
        public static async Task<ApiFilterRunResult> RunFiltersAndCheckIfShouldContinue(HttpContext httpCtx, ActionContext action)
        {
            if (action.SkipAuth)
            {
                var nonSkipable = action.ParentController.Filters.Where(x => x.IgnoreSkipFilter);
                foreach (var filter in nonSkipable)
                {
                    var shouldContinue = await filter.ShouldContinueAsync(httpCtx);
                    if (!shouldContinue.ShouldContinue)
                    {
                        return shouldContinue;
                    }
                }
                return new ApiFilterRunResult { ShouldContinue = true };
            }

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
