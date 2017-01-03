using LiteApi.Contracts.Abstractions;
using LiteApi.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        internal IAuthorizationPolicyStore AuthorizationPolicyStore = new AuthorizationPolicyStore();

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
        /// Gets the JSON serializer. Implementation of <see cref="IJsonSerializer"/>
        /// </summary>
        /// <value>
        /// The JSON serializer. Implementation of <see cref="IJsonSerializer"/>
        /// </value>
        public IJsonSerializer JsonSerializer { get; private set; } = new JsonSerializer();

        /// <summary>
        /// Gets the logger factory.
        /// </summary>
        /// <value>
        /// The logger factory.
        /// </value>
        public ILoggerFactory LoggerFactory { get; private set; }

        /// <summary>
        /// Sets the JSON serializer.
        /// </summary>
        /// <param name="jsonSerializer">The JSON serializer.</param>
        /// <returns>This instance</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public LiteApiOptions SetJsonSerializer(IJsonSerializer jsonSerializer)
        {
            if (jsonSerializer == null) throw new System.ArgumentNullException(nameof(jsonSerializer));
            JsonSerializer = jsonSerializer;
            return this;
        }

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
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public LiteApiOptions SetLoggerFactory(ILoggerFactory loggerFactory)
        {
            if (loggerFactory == null) throw new ArgumentNullException(nameof(loggerFactory));
            LoggerFactory = loggerFactory;
            return this;
        }
    }
}
