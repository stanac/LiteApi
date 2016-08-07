using LiteApi.Contracts.Models;
using System.Reflection;

namespace LiteApi.Contracts.Abstractions
{
    public interface IControllerDiscoverer
    {
        ControllerContext[] GetControllers(Assembly assembly);
    }
}
