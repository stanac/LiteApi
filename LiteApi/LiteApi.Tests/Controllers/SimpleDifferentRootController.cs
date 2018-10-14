namespace LiteApi.Tests.Controllers
{
    [ControllerRoute("theApi/SimpleDifferentRoot")]
    public class SimpleDifferentRootController : LiteController
    {
        public string Get()
        {
            return "SimpleDifferentRoot";
        }
    }
}
