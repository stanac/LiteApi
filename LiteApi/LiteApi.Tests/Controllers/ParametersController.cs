namespace LiteApi.Tests.Controllers
{
    public class ParametersController : LiteController
    {
        public string ToUpper(string a = "abc")
        {
            return (a ?? "").ToUpper();
        }
    }
}
