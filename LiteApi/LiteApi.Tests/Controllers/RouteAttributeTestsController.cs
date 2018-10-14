namespace LiteApi.Tests.Controllers
{
    [ControllerRoute("SomeOtherRoute/RouteAttributeTests")]
    public class RouteAttributeTestsController : LiteController
    {
     //   [Route("Add")]
        public int Method(int a, int b) => a + b;
    }
}
