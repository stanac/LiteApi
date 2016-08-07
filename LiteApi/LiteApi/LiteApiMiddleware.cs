using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace LiteApi
{
    public class LiteApiMiddleware
    {
        private RequestDelegate _next;

        internal static LiteMiddlewareOptions Options { get; private set; }
        internal static bool IsRegistered { get; private set; }
        internal static IServiceProvider Services;

        public LiteApiMiddleware(RequestDelegate next, LiteMiddlewareOptions options, IServiceProvider services)
        {
            if (IsRegistered) throw new Exception("Middleware is already registered.");

            if (options == null) throw new ArgumentNullException(nameof(options));
            if (options.ControllerAssemblies?.Count == 0)
            {
                throw new ArgumentException("Assemblies with controllers is not passed to the LiteApiMiddleware");
            }
            Options = options;
            Services = services;
            _next = next;
            IsRegistered = true;
        }

        public async Task Invoke(HttpContext context, ILoggerFactory loggerFactory)
        {
            Debug.WriteLine("12");

            await _next?.Invoke(context);
        }
        
        public void AddControllerAssemblies(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                AddControllerAssembly(assembly);
            }
        }

        public void AddControllerAssembly(Assembly assembly)
        {
            if (!Options.ControllerAssemblies.Any(x => x.FullName == assembly.FullName))
            {
                Options.ControllerAssemblies.Add(assembly);
            }
        }

        public void ReloadControllers()
        {

        }
    }
}
