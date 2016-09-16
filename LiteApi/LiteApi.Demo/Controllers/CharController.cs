using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Demo.Controllers
{
    public class CharController : LiteController
    {
        public string SumChars(IEnumerable<char> c)
        {
            return new string(c.ToArray());
        }
    }
}
