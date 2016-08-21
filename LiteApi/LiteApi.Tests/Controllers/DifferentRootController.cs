using LiteApi.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Tests.Controllers
{
    [UrlRoot("/api/v2/")]
    public class DifferentRootController : LiteController
    {
        public string Get()
        {
            return "DifferentRootController";
        }
    }
}
