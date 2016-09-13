using LiteApi.Attributes;
#pragma warning disable RECS0154 // Parameter is never used

namespace LiteApi.Tests.Controllers
{
    public class DifferentHttpMethodsController : LiteController
    {
        public string Action()
        {
            return "default get";
        }

        [HttpGet]
        public string Action(int a)
        {
            return "get";
        }

        [HttpPost]
        public string Action(int a, int b)
        {
            return "post";
        }

        [HttpPut]
        public string Action(int a, int b, int c)
        {
            return "put";
        }

        [HttpDelete]
        public string Action(string d)
        {
            return "delete";
        }
    }
}
#pragma warning restore RECS0154 // Parameter is never used
