using LiteApi.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;
using System;

namespace LiteApi.Tests.Controllers
{
    public class SecureController2 : LiteController
    {
        [BadFilterAttribute()]
        public int Get14()
        {
            return 14;
        }

        [AttributeUsage(AttributeTargets.Method)]
        private class BadFilterAttribute : Attribute, IApiFilter
        {
            public ApiFilterRunResult ShouldContinue(HttpContext httpCtx)
            {
                // filter is not setting status code or message, it's used to check if 
                // action invoker can set valid response code and message
                return new ApiFilterRunResult { ShouldContinue = false };
            }
        }
    }
}
