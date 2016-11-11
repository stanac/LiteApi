using LiteApi.Attributes;

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

        [RequiresAuthorizationPolicy("Over16")]
        public int Get5()
        {
            return 5;
        }

        [RequiresAuthorizationPolicy("Over18")]
        public int Get6()
        {
            return 6;
        }

        [RequiresClaimWithValues("claimType1", "true")]
        [RequiresClaimWithValues("claimType2", "true")]
        public int Get7()
        {
            return 7;
        }
    }

}
