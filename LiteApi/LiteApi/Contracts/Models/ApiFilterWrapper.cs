using LiteApi.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace LiteApi.Contracts.Models
{
    public class ApiFilterWrapper
    {
        public IApiFilter ApiFilter { get; private set; }
        public IApiFilterAsync ApiFilterAsync { get; private set; }
        
        public bool IgnoreSkipFilter
        {
            get
            {
                if (ApiFilterAsync != null) return ApiFilterAsync.IgnoreSkipFilters;
                return ApiFilter.IgnoreSkipFilters;
            }
        }

        public ApiFilterWrapper(IApiFilter apiFilter)
        {
            if (apiFilter == null) throw new ArgumentNullException(nameof(apiFilter));
            ApiFilter = apiFilter;
        }

        public ApiFilterWrapper(IApiFilterAsync apiFilter)
        {
            if (apiFilter == null) throw new ArgumentNullException(nameof(apiFilter));
            ApiFilterAsync = apiFilter;
        }

        public ApiFilterWrapper(IPolicyApiFilter policyFilter, Func<IAuthorizationPolicyStore> policyStoreFactory)
        {
            ApiFilter = new PolicyFilter(policyFilter, policyStoreFactory);
        }
        
        public async Task<ApiFilterRunResult> ShouldContinueAsync(HttpContext httpCtx)
        {
            if (ApiFilterAsync != null)
            {
                return await ApiFilterAsync.ShouldContinueAsync(httpCtx);
            }
            return ApiFilter.ShouldContinue(httpCtx);
        }

        private class PolicyFilter : IApiFilter
        {
            readonly IPolicyApiFilter _policyFilter;
            readonly Func<IAuthorizationPolicyStore> _policyStoreFactory;

            public PolicyFilter(IPolicyApiFilter policyFilter, Func<IAuthorizationPolicyStore> policyStoreFactory)
            {
                if (policyStoreFactory == null)
                    throw new ArgumentNullException(nameof(policyStoreFactory));
                if (policyFilter == null)
                    throw new ArgumentNullException(nameof(policyFilter));

                _policyStoreFactory = policyStoreFactory;
                _policyFilter = policyFilter;
            }

            public bool IgnoreSkipFilters => false;

            public ApiFilterRunResult ShouldContinue(HttpContext httpCtx)
                => _policyFilter.ShouldContinue(httpCtx.User, _policyStoreFactory);
        }
    }
}
