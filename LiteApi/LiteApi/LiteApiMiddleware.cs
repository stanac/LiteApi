using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using LiteApi.Contracts.Abstractions;
using LiteApi.Services;
using LiteApi.Contracts.Models;

namespace LiteApi
{
    public class LiteApiMiddleware
    {
        private RequestDelegate _next;
        private IPathResolver _pathResolver;
        private IActionInvoker _actionInvoker;

        internal static LiteApiOptions Options { get; private set; } = LiteApiOptions.Default;
        internal static bool IsRegistered { get; private set; }
        internal static IServiceProvider Services;


        public LiteApiMiddleware(RequestDelegate next, LiteApiOptions options, IServiceProvider services)
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
            
            Initialize();
        }

        public async Task Invoke(HttpContext context, ILoggerFactory loggerFactory)
        {
            ActionContext action = _pathResolver.ResolvePath(context.Request);
            if (action == null)
            {
                await _next?.Invoke(context);
            }
            else
            {
                await _actionInvoker.Invoke(context, action);
            }
        }
        
        private void AddControllerAssemblies(IEnumerable<Assembly> assemblies)
        {
            foreach (var assembly in assemblies)
            {
                AddControllerAssembly(assembly);
            }
        }

        private void AddControllerAssembly(Assembly assembly)
        {
            if (!Options.ControllerAssemblies.Any(x => x.FullName == assembly.FullName))
            {
                Options.ControllerAssemblies.Add(assembly);
            }
        }

        private void Initialize()
        {
            IParametersDiscoverer parameterDiscoverer = new ParametersDiscoverer();
            IActionDiscoverer actionDiscoverer = new ActionDiscoverer(parameterDiscoverer);
            IControllerDiscoverer ctrlDiscoverer = new ControllerDiscoverer(actionDiscoverer);

            List<ControllerContext> ctrlContexts = new List<ControllerContext>();

            foreach (var assembly in Options.ControllerAssemblies)
            {
                ctrlContexts.AddRange(ctrlDiscoverer.GetControllers(assembly));
            }

            var actions = ctrlContexts.SelectMany(x => x.Actions).ToArray();

            _pathResolver = new PathResolver(ctrlContexts.ToArray());

            IControllerBuilder ctrlBuilder = new ControllerBuilder();
            IModelBinder modelBinder = new ModelBinder();
            
            _actionInvoker = new ActionInvoker(ctrlBuilder, modelBinder);

            var validator = new ControllersValidator(new ActionsValidator(new ParametersValidator()));
            var errors = validator.GetValidationErrors(ctrlContexts.ToArray()).ToArray();
            if (errors.Any())
            {
                throw new LiteApiRegistrationException($"Failed to initialize {nameof(LiteApiMiddleware)}, see property Errors.", errors);
            }
        }
    }
}
