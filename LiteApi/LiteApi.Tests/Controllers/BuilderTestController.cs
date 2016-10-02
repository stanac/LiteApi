using LiteApi.Attributes;

namespace LiteApi.Tests.Controllers
{
    public class BuilderTestController : LiteController
    {
        public string StrVal { get; set; }

        [ApiConstructor]
        public BuilderTestController()
        {
            StrVal = "default";
        }

        public BuilderTestController(string isSet)
        {
            StrVal = isSet;
        }
    }
}
