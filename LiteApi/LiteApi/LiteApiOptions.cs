using LiteApi.Contracts.Abstractions;
using LiteApi.Services;
using System.Collections.Generic;
using System.Reflection;

namespace LiteApi
{
    public class LiteApiOptions
    {
        public LiteApiOptions()
        {
            ControllerAssemblies.Add(Assembly.GetEntryAssembly());
        }

        public List<Assembly> ControllerAssemblies { get; } = new List<Assembly>();

        public static LiteApiOptions Default => new LiteApiOptions();

        public IJsonSerializer JsonSerializer { get; private set; } = new JsonSerializer();

        public LiteApiOptions SetJsonSerializer(IJsonSerializer jsonSerializer)
        {
            if (jsonSerializer == null) throw new System.ArgumentNullException(nameof(jsonSerializer));
            JsonSerializer = jsonSerializer;
            return this;
        }
    }
}
