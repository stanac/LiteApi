using LiteApi.Attributes;

namespace LiteApi.Tests.Controllers
{
    [UrlRoot(null)]
    public class NoRootController : LiteController
    {
        public string Get()
        {
            return "NoRoot";
        }
    }
}
