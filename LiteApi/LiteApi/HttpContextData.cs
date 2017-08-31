using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace LiteApi
{
    internal static class HttpContextData
    {
        private const string ResponseCodeKey = "LiteApi.HttpContextData.ResponseCode";
        private const string ResponseHeadersKey = "LiteApi.HttpContextData.ResponseHeaders";
        private const string LiteApiOptionsKey = "LiteApi.LiteApiOptions";
        private const string ActionContextKey = "LiteApi.Contracts.Models.ActionContext";

        public static int? GetResponseStatusCode(this HttpContext ctx)
        {
            if (ctx.Items.ContainsKey(ResponseCodeKey))
            {
                return ctx.Items[ResponseCodeKey] as int?;
            }
            return null;
        }

        public static void SetResponseStatusCode(this HttpContext ctx, int? responseCode)
        {
            ctx.Items[ResponseCodeKey] = responseCode;
        }

        public static Dictionary<string, StringValues> GetResponseHeaders(this HttpContext ctx, bool getNullIfMissing)
        {
            if (!ctx.Items.ContainsKey(ResponseHeadersKey))
            {
                if (getNullIfMissing) return null;

                ctx.Items[ResponseHeadersKey] = new Dictionary<string, StringValues>();
            }
            return ctx.Items[ResponseHeadersKey] as Dictionary<string, StringValues>;
        }

        public static void SetLiteApiOptions(this HttpContext ctx, LiteApiOptions options)
        {
            ctx.Items[LiteApiOptionsKey] = options;
        }

        public static LiteApiOptions GetLiteApiOptions(this HttpContext ctx)
            => ctx.Items[LiteApiOptionsKey] as LiteApiOptions;

        public static void SetActionContext(this HttpContext ctx, ActionContext actionCtx)
        {
            ctx.Items[ActionContextKey] = actionCtx;
        }

        public static ActionContext GetActionContext(this HttpContext ctx)
        {
            object actionCtx = null;
            ctx.Items.TryGetValue(ActionContextKey, out actionCtx);
            if (actionCtx == null) return null;
            return actionCtx as ActionContext;
        }
    }
}
