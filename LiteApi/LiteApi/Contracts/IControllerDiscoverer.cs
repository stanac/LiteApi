using System.Reflection;

namespace LiteApi.Contracts
{
    public interface IControllerDiscoverer
    {
        ControllerContext[] GetControllers(Assembly assembly);
    }
}
