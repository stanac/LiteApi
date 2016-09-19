using LiteApi.Attributes;
using System.Security.Claims;

namespace LiteApi.Tests.Controllers
{
    [UrlRoot("/complex/root/with/multiple/parts/")]
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
}
