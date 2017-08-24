using LiteApi.Services.Discoverers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteApi.Contracts.Abstractions;

namespace LiteApi.Demo
{
    public class CustomControllerDiscoverer : ControllerDiscoverer
    {
        public CustomControllerDiscoverer(IActionDiscoverer actionDiscoverer, ILiteApiOptionsRetriever optionsRetriever) : base(actionDiscoverer, optionsRetriever)
        {
        }

        protected override string GetControllerName(string typeFullName)
        {
            var name = base.GetControllerName(typeFullName);
            if (name.EndsWith("ctrl", StringComparison.Ordinal))
                name = name.Substring(0, name.Length - 4);
            return name;
        }
    }
}
