using LiteApi.Attributes;
using System;
using System.Collections.Generic;
#pragma warning disable RECS0154 // Parameter is never used

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

        public string Get5(IEnumerable<char> data)
        {
            return "IEnumerable<char>";
        }

        public string Get6(IDictionary<int, string> data)
        {
            return "IDictionary<int, string>";
        }

        [HttpPost]
        public string Post7(IEnumerable<Tuple<int, string>> data)
        {
            return "IEnumerable<Tuple<int, string>> data";
        }
    }
}
#pragma warning restore RECS0154 // Parameter is never used
