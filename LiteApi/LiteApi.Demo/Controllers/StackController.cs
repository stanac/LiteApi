using System.Collections.Generic;
using System.Linq;

namespace LiteApi.Demo.Controllers
{
    public class StackCtrl: LiteController 
    // it's possible for controller name to end in Ctrl and respond to "/api/stack" since we replaced default controller discoverer in Startup.cs
    {
        public int NonNullCount([FromQuery]Stack<int?> s) => s.Count(x => x.HasValue);
    }
}
