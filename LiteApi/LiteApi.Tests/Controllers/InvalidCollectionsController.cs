using System;
using System.Collections.Generic;

namespace LiteApi.Tests.Controllers
{
    public class InvalidCollectionsController : LiteController
    {
        public string InvalidCollectionsGet1(List<Tuple<int, int>> p1)
        {
            return "List<Tuple<int, int>>";
        }

        public string InvalidCollectionsGet2(Dictionary<Tuple<int, string>, int> p1)
        {
            return "Dictionary<int, int>";
        }

        public string InvalidCollectionsGet3(LiteController[] p1)
        {
            return "LiteController[]";
        }
    }
}
