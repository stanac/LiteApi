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
using Microsoft.AspNetCore.Builder;
using LiteApi.Services.ModelBinders;
using LiteApi.Services.Validators;
using LiteApi.Services.Discoverers;
using LiteApi.Services.Logging;

namespace LiteApi
{
    /// <summary>
    /// Middleware to register with ASP.NET Core
    /// </summary>
    public class LiteApiMiddleware
    {
        private RequestDelegate _next;
        private IPathResolver _pathResolver;
        private IActionInvoker _actionInvoker;
        private ILogger _logger;
        private bool _isLoggingEnabled;

        internal static LiteApiOptions Options { get; private set; } = LiteApiOptions.Default;
        internal static bool IsRegistered { get; private set; }
        internal static IServiceProvider Services { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteApiMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next, provided by ASP.NET</param>
        /// <param name="options">The options, passed by <see cref="IApplicationBuilder"/> extension method.</param>
        /// <param name="services">The services, provided by ASP.NET</param>
        /// <exception cref="System.Exception">Middleware is already registered.</exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentException">Assemblies with controllers is not passed to the LiteApiMiddleware</exception>
        public LiteApiMiddleware(RequestDelegate next, LiteApiOptions options, IServiceProvider services)
        {
            if (IsRegistered) throw new Exception("Middleware is already registered.");

            if (options == null) throw new ArgumentNullException(nameof(options));
            if (options.ControllerAssemblies?.Count == 0)
            {
                throw new ArgumentException("Assemblies with controllers is not passed to the LiteApiMiddleware");
            }
            Options = options;
            if (options.LoggerFactory != null)
            {
                _logger = new InternalLogger(true, options.LoggerFactory.CreateLogger<LiteApiMiddleware>());
                _isLoggingEnabled = true;
            }
            else
            {
                _logger = new InternalLogger(false, null);
            }
            Services = services;
            _next = next;
            IsRegistered = true;            
            
            Initialize();
        }

        /// <summary>
        /// Gets called by ASP.NET framework
        /// </summary>
        /// <param name="context">Instance of <see cref="HttpContext"/> provided by ASP.NET framework</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            ILogger log = new ContextAwareLogger(_isLoggingEnabled, _logger, context.TraceIdentifier);
            log.LogInformation($"Received request: {context.Request.Path} with query: {context.Request.QueryString.ToString() ?? ""}");
            ActionContext action = _pathResolver.ResolveAction(context.Request, log);
            if (action == null)
            {
                log.LogInformation("Request is skipped to next middleware");
                if (_next != null)
                {
                    log.LogInformation("Invoking next middleware");
                    await _next?.Invoke(context);
                    log.LogInformation("Next middleware invoked");
                }
                else
                {
                    log.LogInformation("There is no next middleware to invoke");
                }
            }
            else
            {
                await _actionInvoker.Invoke(context, action, log);
                log.LogInformation("Action is invoked");
            }
            log.LogInformation("Request is processed");
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
            _logger.LogInformation("LiteApi middleware initialization started");
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
            ModelBinderCollection modelBinder = new ModelBinderCollection(Options.JsonSerializer);
            foreach (IQueryModelBinder qmb in Options.AdditionalQueryModelBinders)
            {
                modelBinder.AddAdditionalQueryModelBinder(qmb);
            }
            
            _actionInvoker = new ActionInvoker(ctrlBuilder, modelBinder);

            var authPolicyStore = Options.AuthorizationPolicyStore;
            var validator = new ControllersValidator(new ActionsValidator(new ParametersValidator(), authPolicyStore), authPolicyStore);
            var errors = validator.GetValidationErrors(ctrlContexts.ToArray()).ToArray();
            if (errors.Any())
            {
                _logger.LogError("One or more errors occurred while initializing LiteApi middleware. Check next log entry/entries.");
                foreach (var error in errors)
                {
                    _logger.LogError(error);
                }
                throw new LiteApiRegistrationException($"Failed to initialize {nameof(LiteApiMiddleware)}, see property Errors or log if enabled.", errors);
            }
        }
    }
}
