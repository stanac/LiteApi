using System;
using System.Collections.Generic;

namespace LiteApi.Tests.Controllers
{
    public class CollectionController : LiteController
    {
        public string Get1(string[] data)
        {
            return "string[]";
        }

        public string Get2(List<int> data)
        {
            return "List<int>";
        }

        public string Get3(List<int?> data)
        {
            return "List<int?>";
        }

        public string Get4(List<Guid?> data)
        {
            return "List<Guid?>";
        }
    }
}
