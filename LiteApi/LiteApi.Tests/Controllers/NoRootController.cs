using LiteApi.Attributes;

namespace LiteApi.Tests.Controllers
{
    [ControllerRoute("NoRoot")]
    public class NoRootController : LiteController
    {
        public string Get()
        {
            return "NoRoot";
        }
    }
}
