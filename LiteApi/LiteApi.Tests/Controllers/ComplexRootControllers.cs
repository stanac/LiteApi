using LiteApi.Attributes;
using System.Security.Claims;

namespace LiteApi.Tests.Controllers
{
    [ControllerRoute("/complex/root/with/multiple/parts/ComplexRoot")]
    public class ComplexRootController : LiteController
    {
        public string Get()
        {
            return "ComplexRoot";
        }

        public ClaimsPrincipal GetUser()
        {
            return User;
        }
    }

    [ControllerRoute("/complex/root/with/multiple/parts")]
    public class ComplexRoot2Controller : LiteController
    {
        public string Get()
        {
            return "ComplexRoot2";
        }

        public ClaimsPrincipal GetUser()
        {
            return User;
        }
    }
}
