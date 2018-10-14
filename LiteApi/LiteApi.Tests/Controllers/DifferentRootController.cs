namespace LiteApi.Tests.Controllers
{
    [ControllerRoute("/api/v2/DifferentRoot")]
    public class DifferentRootController : LiteController
    {
        public string Get()
        {
            return "DifferentRootController";
        }
    }
}
