using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using LiteApi.Services;
using LiteApi.Services.Discoverers;
using LiteApi.Services.Logging;
using LiteApi.Services.ModelBinders;
using LiteApi.Services.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LiteApi
{
    /// <summary>
    /// Middleware to register with ASP.NET Core
    /// </summary>
    public class LiteApiMiddleware
    {
        private RequestDelegate _next;
        private ILogger _logger;
        private bool _isLoggingEnabled;

        // TODO: remove static from Options
        internal LiteApiOptions Options { get; private set; } = LiteApiOptions.Default;
        internal static bool IsRegistered { get; private set; }

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
            Options = options ?? throw new ArgumentNullException(nameof(options));

            options.InternalServiceResolver.Initialize(services, options);

            if (options.ControllerAssemblies?.Count == 0)
            {
                throw new ArgumentException("Assemblies with controllers is not passed to the LiteApiMiddleware");
            }
            if (options.LoggerFactory != null)
            {
                _logger = new InternalLogger(true, options.LoggerFactory.CreateLogger<LiteApiMiddleware>());
                _isLoggingEnabled = true;
            }
            else
            {
                _logger = new InternalLogger(false, null);
            }
            // Services = services;
            _next = next;
            IsRegistered = true;            
            
            Initialize(services);
        }

        /// <summary>
        /// Gets called by ASP.NET framework
        /// </summary>
        /// <param name="httpCtx">Instance of <see cref="HttpContext"/> provided by ASP.NET framework</param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpCtx)
        {
            ILogger log = new ContextAwareLogger(_isLoggingEnabled, _logger, httpCtx.TraceIdentifier);
            log.LogInformation($"Received request: {httpCtx.Request.Path} with query: {httpCtx.Request.QueryString.ToString() ?? ""}");
            IPathResolver pathResolver = Options.InternalServiceResolver.GetPathResolver();
            ActionContext action = pathResolver.ResolveAction(httpCtx.Request, log);
            if (action == null)
            {
                log.LogInformation("Request is skipped to next middleware");
                if (_next != null)
                {
                    log.LogInformation("Invoking next middleware");
                    await _next?.Invoke(httpCtx);
                    log.LogInformation("Next middleware invoked");
                }
                else
                {
                    log.LogInformation("There is no next middleware to invoke");
                }
            }
            else
            {
                // TODO: consider replacing RequiresHttps with global filter
                if (Options.RequiresHttps && !httpCtx.Request.IsHttps)
                {
                    log.LogInformation("LiteApi options are set to require HTTPS, request rejected because request is HTTP");
                    httpCtx.Response.StatusCode = 400;
                    await httpCtx.Response.WriteAsync("Bad request, HTTPS request was expected.");
                }
                else
                {
                    if (!(await CheckGlobalFiltersAndWriteResponseIfAny(httpCtx, log, action.SkipAuth)))
                    {
                        return;
                    }
                    var actionInvoker = Options.InternalServiceResolver.GetActionInvoker();
                    await actionInvoker.Invoke(httpCtx, action, log);
                    log.LogInformation("Action is invoked");
                }
            }
            log.LogInformation("Request is processed");
        }
        
        private void Initialize(IServiceProvider services)
        {
            _logger.LogInformation("LiteApi middleware initialization started");
            Options.InternalServiceResolver.RegisterInstance<IAuthorizationPolicyStore>(Options.AuthorizationPolicyStore);

            IControllerDiscoverer ctrlDiscoverer = Options.InternalServiceResolver.GetControllerDiscoverer();

            List<ControllerContext> ctrlContexts = new List<ControllerContext>();

            foreach (var assembly in Options.ControllerAssemblies)
            {
                ctrlContexts.AddRange(ctrlDiscoverer.GetControllers(assembly));
            }

            var actions = ctrlContexts.SelectMany(x => x.Actions).ToArray();

            IControllerBuilder ctrlBuilder = Options.InternalServiceResolver.GetControllerBuilder();
            ModelBinderCollection modelBinder = new ModelBinderCollection(Options.InternalServiceResolver.GetJsonSerializer(), services);
            foreach (IQueryModelBinder qmb in Options.AdditionalQueryModelBinders)
            {
                modelBinder.AddAdditionalQueryModelBinder(qmb);
            }
            
            var authPolicyStore = Options.AuthorizationPolicyStore;
            IControllersValidator validator = Options.InternalServiceResolver.GetControllerValidator();
            var errors = validator.GetValidationErrors(ctrlContexts.ToArray()).ToArray();
            if (errors.Any())
            {
                _logger.LogError("One or more errors occurred while initializing LiteApi middleware. Check next log entry/entries.");
                foreach (var error in errors)
                {
                    _logger.LogError(error);
                }
                string allErrors = "\n\n --------- \n\n" + string.Join("\n\n --------- \n\n", errors);
                throw new LiteApiRegistrationException($"Failed to initialize {nameof(LiteApiMiddleware)}, see property Errors, log if enabled, or check erros listed below." + allErrors, errors);
            }

            Func<Type, bool> isRegistered = (type) => Options.InternalServiceResolver.IsServiceRegistered(type);
            
            if (!isRegistered(typeof(IPathResolver)))
                Options.InternalServiceResolver.RegisterInstance<IPathResolver>(new PathResolver(ctrlContexts.ToArray(), Options.InternalServiceResolver.GetOptionsRetriever()));

            if (!isRegistered(typeof(IModelBinder)))
                Options.InternalServiceResolver.RegisterInstance<IModelBinder>(modelBinder);
        }

        private async Task<bool> CheckGlobalFiltersAndWriteResponseIfAny(HttpContext httpCtx, ILogger log, bool actionHasSkipFilter)
        {
            IEnumerable<ApiFilterWrapper> filters = actionHasSkipFilter
                ? Options.GlobalFilters.Where(x => x.IgnoreSkipFilter)
                : Options.GlobalFilters;

            ApiFilterRunResult result;
            foreach (var filter in filters)
            {
                result = await filter.ShouldContinueAsync(httpCtx);
                if (!result.ShouldContinue)
                {
                    int code = result.SetResponseCode ?? 401;
                    string message = result.SetResponseMessage ?? "unauthorized request";
                    httpCtx.Response.StatusCode = code;
                    await httpCtx.Response.WriteAsync(message);
                    log.LogInformation($"Global filter {filter.GetFilterTypeName()} return false for should continue.");
                    return false;
                }
            }
            return true;
        }
    }
}
