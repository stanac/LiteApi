namespace LiteApi.Tests.Controllers
{
    [ControllerRoute("/api/v2/Route")]
    public class RouteParamsController : LiteController
    {
        [ActionRoute("{a}/Plus/{b}")]
        public int Sum([FromRoute]int a, [FromRoute]int b, int c = 0)
        {
            return a + b + c;
        }

        [ActionRoute("{a}/Plus2/{b}")]
        public int Sum2(int a, int b, int c = 0)
        {
            return a + b + c;
        }

        [ActionRoute("{valueAa}/Plus3/{valueBb}")]
        public int Sum3(int valueAa, int valueBb)
            => valueAa + valueBb;
    }
}
