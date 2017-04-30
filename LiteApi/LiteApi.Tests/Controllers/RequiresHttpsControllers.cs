using LiteApi.Attributes;

namespace LiteApi.Tests.Controllers
{
    [RequiresHttps]
    public class RequiresHttpsWithoutIgnoreSkipController : LiteController
    {
        public int Get1() => 1;

        [SkipFilters]
        public int Get2() => 2;
    }

    [RequiresHttps(IgnoreSkipFilters = false)]
    public class RequiresHttpsWithIgnoreSkipController : LiteController
    {
        public int Get1() => 1;

        [SkipFilters]
        public int Get2() => 2;
    }

    public class RequiresHttpsOnActionController : LiteController
    {
        public int Get1() => 1;

        [RequiresHttps]
        public int Get2() => 2;
    }

}
