using LiteApi.Attributes;
using System.Linq;

namespace LiteApi.Demo.Controllers
{
    public class HeaderParametersController: LiteController
    {
        // parameter values will be retrieved from headers "i" and "x-overriden-param-name-j"
        public int Add([FromHeader]int i, [FromHeader("x-overriden-param-name-j")]int j) => i + j;

        public int Sum([FromHeader]int[] i) => i.Sum();

        public bool IsNull([FromHeader]int? a) => !a.HasValue;

        public string StringLength([FromHeader]string s) => s == null ? "it is null" : $"string not null, length: {s.Length}";
    }
}
