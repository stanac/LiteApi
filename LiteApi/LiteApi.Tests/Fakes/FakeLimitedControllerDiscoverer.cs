using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using System.Reflection;
using LiteApi.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using LiteApi.Services.Discoverers;

namespace LiteApi.Tests.Fakes
{
    public class FakeLimitedControllerDiscoverer : IControllerDiscoverer
    {
        private readonly IControllerDiscoverer _impl = new ControllerDiscoverer(new ActionDiscoverer(new ParametersDiscoverer(new Moq.Mock<IServiceProvider>().Object)));
        private Type[] _controllersToDiscover;

        public FakeLimitedControllerDiscoverer(params Type[] controllersToDiscover)
        {
            if (controllersToDiscover == null) throw new ArgumentNullException(nameof(controllersToDiscover));

            _controllersToDiscover = controllersToDiscover;
        }

        public ControllerContext[] GetControllers(Assembly assembly)
        {
            List<ControllerContext> ctrls = new List<ControllerContext>();
            var assemblies = _controllersToDiscover.Select(x => x.GetTypeInfo().Assembly).Distinct().ToArray();
            foreach (var asm in assemblies)
            {
                ctrls.AddRange(_impl.GetControllers(asm));
            }
            return ctrls.Where(x => _controllersToDiscover.Contains(x.ControllerType)).ToArray();
        }
    }
}
