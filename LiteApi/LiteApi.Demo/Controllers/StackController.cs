using LiteApi.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi.Demo.Controllers
{
    public class StackController: LiteController
    {
        public int NonNullCount([FromQuery]Stack<int?> s) => s.Count(x => x.HasValue);
    }
}
