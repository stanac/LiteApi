using LiteApi.Attributes;

namespace LiteApi.Tests.Controllers
{
    [ControllerRoute("DifferentNoRoot")]
    public class DifferentNoRootController : LiteController
    {
        public string Get()
        {
            return "DifferentNoRoot";
        }
    }
}
