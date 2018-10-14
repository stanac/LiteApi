namespace LiteApi.Demo.Controllers
{
    public class HttpsTestController: LiteController
    {
        [RequiresHttps]
        public int Get1() => 1;

        public int Get2() => 2;
    }
}
