using LiteApi.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Tests.Controllers
{
    [UrlRoot("/complex/root/with/multiple/parts/")]
    public class ComplexRootController : LiteController
    {
        public string Get()
        {
            return "ComplexRoot";
        }
    }
}
