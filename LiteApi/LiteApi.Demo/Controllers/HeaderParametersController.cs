using LiteApi.Attributes;

namespace LiteApi.Demo.Controllers
{
    public class HeaderParametersController: LiteController
    {
        // parameter values will be retrieved from headers "i" and "x-overriden-param-name-j"
        public int Add([FromHeader]int i, [FromHeader("x-overriden-param-name-j")]int j) => i + j;
    }
}
