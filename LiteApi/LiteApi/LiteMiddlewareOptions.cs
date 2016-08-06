using System.Collections.Generic;
using System.Reflection;

namespace LiteApi
{
    public class LiteMiddlewareOptions
    {
        public LiteMiddlewareOptions()
        {
            ControllerAssemblies.Add(Assembly.GetEntryAssembly());
        }

        public List<Assembly> ControllerAssemblies { get; } = new List<Assembly>();
        
        public static LiteMiddlewareOptions Default => new LiteMiddlewareOptions();

        
    }
}
