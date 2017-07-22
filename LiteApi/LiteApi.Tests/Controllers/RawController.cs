using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteApi.Tests.Controllers
{
    public class RawController: LiteController
    {
        public ILiteActionResult SomeResponse()
        {
            AddResponseHeader("key", "val");
            SetResponseStatusCode(241);
            return Json("{ \"a\": \"b\" }");
        }
    }
}
