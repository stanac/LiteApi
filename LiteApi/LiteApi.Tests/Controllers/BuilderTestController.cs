namespace LiteApi.Tests.Controllers
{
    public class BuilderTestController : LiteController
    {
        public string StrVal { get; set; }

        [PrimaryConstructor]
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
