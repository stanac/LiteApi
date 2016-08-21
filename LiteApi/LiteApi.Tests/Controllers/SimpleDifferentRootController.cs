using LiteApi.Attributes;

namespace LiteApi.Tests.Controllers
{
    [UrlRoot("theApi")]
    public class SimpleDifferentRootController : LiteController
    {
        public string Get()
        {
            return "SimpleDifferentRoot";
        }
    }
}
