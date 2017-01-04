using System.Threading;
using System.Threading.Tasks;

namespace LiteApi.Tests.Controllers
{
    public class DifferentMethodTypesController: LiteController
    {
        public void VoidAction()
        {
            Thread.Sleep(1);
        }

        public async Task TaskAction()
        {
            await Task.Delay(1);
        }

        public int IntAction()
        {
            return 1;
        }

        public async Task<int> IntTaskAction()
        {
            await Task.Delay(1);
            return 1;
        }
    }
}
