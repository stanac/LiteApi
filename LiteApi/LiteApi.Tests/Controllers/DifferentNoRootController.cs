using LiteApi.Attributes;

namespace LiteApi.Tests.Controllers
{
    [UrlRoot("")]
    public class DifferentNoRootController : LiteController
    {
        public string Get()
        {
            return "DifferentNoRoot";
        }
    }
}
