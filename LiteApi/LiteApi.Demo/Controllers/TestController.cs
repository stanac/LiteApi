using LiteApi.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi.Demo.Controllers
{
    public class TestController : LiteController
    {
        readonly IDemoService _service;

        public TestController(IDemoService service)
        {
            _service = service;
        }

        public int Add(int a, int b)
        {
            return _service.Add(a, b);
        }

        public string Add(string a, string b)
        {
            return _service.Add(a, b);
        }

        public int SumInts(int[] ints)
        {
            return ints.Sum();
        }
        
        public int Action1(int i)
        {
            return i;
        }

        public int Action1(int[] i)
        {
            return i.Sum();
        }
        
        public int Action2(int i)
        {
            return i;
        }

        public int Action2(int? i)
        {
            return i ?? 0;
        }

        public int Action2(int[] i)
        {
            return i.Sum();
        }

        public int Action2(int?[] i)
        {
            return i.Select(x => x ?? 0).Sum();
        }

        public int Action3(int?[] i)
        {
            return i.Select(x => x ?? 0).Sum();
        }

        public int Action3(int a, int b, [FromServices]IDemoService theService)
        {
            return theService.Add(a, b);
        }

        public object SumNotNullable(List<int?> ints)
        {
            bool hasNulls = ints.Any(x => !x.HasValue);
            int sum = ints.Where(x => x.HasValue).Select(x => x.Value).Sum();
            return new
            {
                hasNulls, sum
            };
        }

        public string EnumToString(Entity e = Entity.Account)
        {
            return e.ToString();
        }

        [ActionRoute("{e}/tostring")]
        public string ToString(Entity e)
        {
            return e.ToString();
        }

        public ILiteActionResult EmpryJsonObject()
        {
            AddResponseHeader("x-stat", "accepted");
            SetResponseStatusCode(204);
            return Json("{ }");
        }
    }

    public enum Entity
    {
        Person, Company, Account
    }
}
