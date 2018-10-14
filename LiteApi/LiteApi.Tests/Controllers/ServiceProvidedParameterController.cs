using System;

namespace LiteApi.Tests.Controllers
{
    public class ServiceProvidedParameterController: LiteController
    {
        public int Increment(int i, [FromServices]IIncrementService service)
        {
            i = service.Increment(i);
            return i;
        }
    }

    public class IncrementService : IIncrementService
    {
        public int Increment(int i)
        {
            return i + 1;
        }
    }

    public interface IIncrementService
    {
        int Increment(int i);
    }
}
