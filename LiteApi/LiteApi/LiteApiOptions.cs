using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using LiteApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace LiteApi
{
    /// <summary>
    /// Options for desired behavior of the middleware
    /// </summary>
    public class LiteApiOptions
    {
        internal List<IQueryModelBinder> AdditionalQueryModelBinders { get; } = new List<IQueryModelBinder>();

        internal IAuthorizationPolicyStore AuthorizationPolicyStore { get; } = new AuthorizationPolicyStore();

        internal List<ApiFilterWrapper> GlobalFilters { get; } = new List<ApiFilterWrapper>();
        
        /// <summary>
        /// Gets the internal service resolver.
        /// </summary>
        /// <value>
        /// The internal service resolver.
        /// </value>
        public ILiteApiServiceResolver InternalServiceResolver { get; private set; } = new LiteApiServiceResolver();

        /// <summary>
        /// Gets the global date time parsing format. Affects <see cref="DateTime"/> and <see cref="DateTimeOffset"/>.
        /// Default is null, which means it will try to parse regardless of the format provided.
        /// </summary>
        /// <value>
        /// The global date time parsing format.
        /// </value>
        public string GlobalDateTimeParsingFormat { get; private set; }

        /// <summary>
        /// Gets the date time parsing format provider factory. Default is <see cref="System.Globalization.CultureInfo.CurrentCulture"/>
        /// </summary>
        /// <value>
        /// The date time parsing format provider factory. Default is <see cref="System.Globalization.CultureInfo.CurrentCulture"/>
        /// </value>
        public Func<HttpContext, IFormatProvider> DateTimeParsingFormatProviderFactory { get; private set; }
            = httpCtx => System.Globalization.CultureInfo.CurrentCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="LiteApiOptions"/> class.
        /// </summary>
        public LiteApiOptions()
        {
            ControllerAssemblies.Add(Assembly.GetEntryAssembly());
        }

        /// <summary>
        /// Gets the controller assemblies.
        /// </summary>
        /// <value>
        /// The controller assemblies.
        /// </value>
        public List<Assembly> ControllerAssemblies { get; } = new List<Assembly>();
        
        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>
        /// The default.
        /// </value>
        public static LiteApiOptions Default => new LiteApiOptions();
        
        /// <summary>
        /// Gets a value indicating whether middleware should reject all non HTTPS requests.
        /// </summary>
        /// <value>
        ///   <c>true</c> if middleware requires HTTPS; otherwise, <c>false</c>.
        /// </value>
        public bool RequiresHttps { get; private set; } = false;

        /// <summary>
        /// Gets the logger factory.
        /// </summary>
        /// <value>
        /// The logger factory.
        /// </value>
        public ILoggerFactory LoggerFactory { get; private set; }

        /// <summary>
        /// Gets the URL root. URL root is root URL route on which API responds, by default it's "api/"
        /// </summary>
        /// <value>
        /// The URL root.
        /// </value>
        public string UrlRoot { get; private set; } = "api/";

        /// <summary>
        /// Gets a value indicating whether to use open API to render swagger.json.
        /// </summary>
        /// <value>
        ///   <c>true</c> if swagger.json should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool UseOpenApi { get; private set; }

        /// <summary>
        /// Adds the controller assemblies.
        /// </summary>
        /// <param name="controllerAssemblies">The controller assemblies.</param>
        /// <returns>This instance</returns>
        public LiteApiOptions AddControllerAssemblies(IEnumerable<Assembly> controllerAssemblies)
        {
            ControllerAssemblies.AddRange(controllerAssemblies);
            return this;
        }
        
        /// <summary>
        /// Adds an additional query model binder.
        /// </summary>
        /// <param name="queryModelBinder">The query model binder.</param>
        /// <returns>This instance</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public LiteApiOptions AddAdditionalQueryModelBinder(IQueryModelBinder queryModelBinder)
        {
            if (queryModelBinder == null) throw new System.ArgumentNullException(nameof(queryModelBinder));
            AdditionalQueryModelBinders.Add(queryModelBinder);
            return this;
        }

        /// <summary>
        /// Adds the authorization policy.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="policy">The policy</param>
        /// <returns>This instance</returns>
        public LiteApiOptions AddAuthorizationPolicy(string name, Func<ClaimsPrincipal, bool> policy)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("name cannot be null or empty or whitespace");
            if (policy == null) throw new ArgumentNullException(nameof(policy));

            AuthorizationPolicyStore.SetPolicy(name, policy);

            return this;
        }

        /// <summary>
        /// Sets the logger factory.
        /// </summary>
        /// <param name="loggerFactory">The logger factory.</param>
        /// <returns>This instance</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public LiteApiOptions SetLoggerFactory(ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            return this;
        }

        /// <summary>
        /// Sets the value indicating if middleware should reject all non HTTPS requests.
        /// </summary>
        /// <param name="requiresHttps">if set to <c>true</c> requires HTTPS.</param>
        /// <returns>This instance</returns>
        public LiteApiOptions SetRequiresHttps(bool requiresHttps)
        {
            RequiresHttps = requiresHttps;
            return this;
        }

        /// <summary>
        /// Replaces the internal service resolver.
        /// </summary>
        /// <param name="serviceResolver">The service resolver.</param>
        /// <returns>This instance</returns>
        public LiteApiOptions ReplaceInternalServiceResolver(ILiteApiServiceResolver serviceResolver)
        {
            InternalServiceResolver = serviceResolver ?? throw new ArgumentNullException(nameof(serviceResolver));
            return this;
        }

        /// <summary>
        /// Adds the global filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>Instance of this options</returns>
        public LiteApiOptions AddGlobalFilter(IApiFilter filter)
        {
            GlobalFilters.Add(new ApiFilterWrapper(filter));
            return this;
        }

        /// <summary>
        /// Adds the global async filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>Instance of this options</returns>
        public LiteApiOptions AddGlobalFilter(IApiFilterAsync filter)
        {
            GlobalFilters.Add(new ApiFilterWrapper(filter));
            return this;
        }

        /// <summary>
        /// Adds the global filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>Instance of this options</returns>
        public LiteApiOptions AddGlobalFilters(IEnumerable<IApiFilter> filters)
        {
            GlobalFilters.AddRange(filters.Select(x => new ApiFilterWrapper(x)));
            return this;
        }

        /// <summary>
        /// Adds the global async filters.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns>Instance of this options</returns>
        public LiteApiOptions AddGlobalFilters(IEnumerable<IApiFilterAsync> filters)
        {
            GlobalFilters.AddRange(filters.Select(x => new ApiFilterWrapper(x)));
            return this;
        }

        /// <summary>
        /// Sets the global date time parsing format. Affects <see cref="DateTime"/> and <see cref="DateTimeOffset"/>.
        /// Default is null, which means it will try to parse regardless of the format provided.
        /// Can be overridden by controller or action with <see cref="Attributes.DateTimeParsingFormatAttribute"/>
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>This instance</returns>
        public LiteApiOptions SetGlobalDateTimeParsingFormat(string format)
        {
            if (format == "") format = null;
            GlobalDateTimeParsingFormat = format;
            return this;
        }

        /// <summary>
        /// Sets the date time parsing format provider factory.
        /// Default is <see cref="System.Globalization.CultureInfo.CurrentCulture" />
        /// </summary>
        /// <param name="formatProviderFactory">The format provider factory.</param>
        /// <returns>This instance</returns>
        public LiteApiOptions SetDateTimeParsingFormatProviderFactory(Func<HttpContext, IFormatProvider> formatProviderFactory)
        {
            DateTimeParsingFormatProviderFactory = formatProviderFactory ?? throw new ArgumentNullException(nameof(formatProviderFactory));
            return this;
        }

        /// <summary>
        /// Sets the API URL root. URL root is root URL route which API responds. Matching is case insensitive.
        /// By default it's /api/.
        /// </summary>
        /// <param name="urlRoot">The URL root. Valid chars are digits and ASCII letters (uppercase or lowercase) and forward slash (/)</param>
        /// <returns>This instance</returns>
        public LiteApiOptions SetApiUrlRoot(string urlRoot)
        {
            const string validChars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789/";
            if (string.IsNullOrWhiteSpace(urlRoot)) throw new ArgumentException("urlRoot cannot be null or empty", nameof(urlRoot));

            urlRoot = urlRoot.Trim().Replace('\\', '/').TrimEnd('/').TrimStart('/');
            string invalidChars = new string(urlRoot.Where(c => !validChars.Contains(c)).Distinct().ToArray());
            if (invalidChars.Any())
            {
                throw new LiteApiRegistrationException($"urlRoot contains invalid chars: {invalidChars} only valid chars are digits and ASCII letters (upper or lowercase) and forward slash (/)");
            }
            UrlRoot = $"{urlRoot}/".ToLower();

            return this;
        }

        /// <summary>
        /// Sets value to decide whether swagger.json should be used.
        /// </summary>
        /// <param name="use">if set to <c>true</c> generates swagger api at [API root]/swagger.json .</param>
        /// <returns>This instance</returns>
        public LiteApiOptions SetUseOpenApi(bool use)
        {
            UseOpenApi = use;
            return this;
        }
    }
}
