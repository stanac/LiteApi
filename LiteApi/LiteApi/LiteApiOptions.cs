using LiteApi.Contracts.Abstractions;
using LiteApi.Services;
using System.Collections.Generic;
using System.Reflection;

namespace LiteApi
{
    /// <summary>
    /// Options for desired behavior of the middleware
    /// </summary>
    public class LiteApiOptions
    {
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
        /// Gets or sets a value indicating whether to enable logging
        /// </summary>
        /// <value>
        ///   <c>true</c> if logging should be enabled, otherwise <c>false</c>.
        /// </value>
        public bool EnableLogging { get; set; }

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
        /// The JSON serializer. Implementation of <see cref="IJsonSerializer"
        /// </value>
        public IJsonSerializer JsonSerializer { get; private set; } = new JsonSerializer();

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
        /// Sets the enable logging.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> logging will be enabled.</param>
        /// <returns>This instance</returns>
        public LiteApiOptions SetEnableLogging(bool enabled)
        {
            EnableLogging = enabled;
            return this;
        }
    }
}
