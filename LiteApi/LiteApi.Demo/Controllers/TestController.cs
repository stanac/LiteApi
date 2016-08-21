namespace LiteApi.Demo.Controllers
{
    public class TestController : LiteController
    {
        readonly IDemoService _service;

        public TestController(IDemoService service)
        {
            _service = service;
        }

        public int Add(int a, int b)
        {
            return _service.Add(a, b);
        }

        public string Add(string a, string b)
        {
            return _service.Add(a, b);
        }
    }
}
