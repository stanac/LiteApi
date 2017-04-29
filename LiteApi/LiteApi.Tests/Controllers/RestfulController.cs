using LiteApi.Attributes;
using System.Collections.Generic;

namespace LiteApi.Tests.Controllers
{
    [Restful]
    public class RestfulController: LiteController
    {
        [HttpGet]
        public int ById(int id)
        {
            return 1 + id;
        }

        [HttpGet, ActionRoute("/{id}")]
        public int ByIdFromRoute(int id)
        {
            return 2 + id;
        }

        [HttpGet]
        public IEnumerable<int> All()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }

        [HttpPost]
        public int Save([FromBody]int i)
        {
            return i - 1;
        }

        [HttpPost, ActionRoute("/{i}")]
        public int SaveFromRoute([FromRoute]int i)
        {
            return i - 2;
        }
    }
}
