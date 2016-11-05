using System;
using System.Threading.Tasks;
using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace LiteApi.Tests.Controllers
{
    [RequiresAuthentication]
    public class SecureController : LiteController
    {
        public int Get1()
        {
            return 1;
        }

        [RequiresRoles("role1", "role2")]
        public int Get2()
        {
            return 2;
        }

        [RequiresClaims("claimType1", "claimType2")]
        public int Get3()
        {
            return 3;
        }

        [SkipFilters]
        public int Get4()
        {
            return 4;
        }

        //[AuthorizeFilter(typeof(UserHasClaim1))]
        //public int Get5()
        //{
        //    return 5;
        //}

        //[AuthorizeFilter(typeof(UserHasClaim2))]
        //public int Get6()
        //{
        //    return 6;
        //}
    }

    //public class UserHasClaim1 : ICustomApiFilter
    //{
    //    public bool IsAsync => false;

    //    public Func<HttpContext, bool> ShouldContinue =>
    //        httpCtx => httpCtx?.User?.Claims.Any(x => x.Type == "claimType1") ?? false;

    //    public Func<HttpContext, Task<bool>> ShouldContinueAsync => null;
    //}

    //public class UserHasClaim2 : ICustomApiFilter
    //{
    //    public bool IsAsync => true;

    //    public Func<HttpContext, bool> ShouldContinue => null;

    //    public Func<HttpContext, Task<bool>> ShouldContinueAsync
    //        => httpCtx => Task.Run(() => httpCtx?.User?.Claims.Any(x => x.Type == "claimType1") ?? false);
    //}
}
