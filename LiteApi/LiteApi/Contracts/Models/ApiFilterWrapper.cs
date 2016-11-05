using LiteApi.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace LiteApi.Contracts.Models
{
    internal class ApiFilterWrapper
    {
        public IApiFilter ApiFilter { get; private set; }
        public IApiFilterAsync ApiFilterAsync { get; private set; }
        public bool IsAsync { get; private set; }

        public ApiFilterWrapper(IApiFilter apiFilter)
        {
            if (apiFilter == null) throw new ArgumentNullException(nameof(apiFilter));
            ApiFilter = apiFilter;
            IsAsync = false;
        }

        public ApiFilterWrapper(IApiFilterAsync apiFilter)
        {
            if (apiFilter == null) throw new ArgumentNullException(nameof(apiFilter));
            ApiFilterAsync = apiFilter;
            IsAsync = true;
        }

        public async Task<ApiFilterRunResult> ShouldContinueAsync(HttpContext httpCtx)
        {
            if (IsAsync)
            {
                return await ApiFilterAsync.ShouldContinueAsync(httpCtx);
            }
            return ApiFilter.ShouldContinue(httpCtx);
        }
    }
}
