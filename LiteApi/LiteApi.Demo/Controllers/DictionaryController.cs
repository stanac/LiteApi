using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Demo.Controllers
{
    public class DictionaryController : LiteController
    {
        public IDictionary<int, string> Join(IDictionary<int, string> a, Dictionary<int, string> b)
        {
            Dictionary<int, string> c = new Dictionary<int, string>();
            foreach (var keyValue in a)
            {
                c[keyValue.Key] = keyValue.Value;
            }
            foreach (var keyValue in b)
            {
                c[keyValue.Key] = keyValue.Value;
            }
            return c;
        }
    }
}
