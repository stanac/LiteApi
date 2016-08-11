using System.Threading.Tasks;

namespace LiteApi.Demo.Controllers
{
    public class TestController : LiteController
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        //public async Task<int> AddAsync(int a, int b)
        //{
        //    await Task.Delay(2000);
        //    return Add(a, b);
        //}
    }
}
