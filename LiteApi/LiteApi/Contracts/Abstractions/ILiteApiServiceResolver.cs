using System;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Internal service resolver used to register and replace LiteApi internal services
    /// </summary>
    public interface ILiteApiServiceResolver
    {
        /// <summary>
        /// Initializes the internal service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        void Initialize(IServiceProvider serviceProvider);

        /// <summary>
        /// Gets the action discoverer.
        /// </summary>
        /// <returns>Instance of <see cref="IActionDiscoverer"/></returns>
        IActionDiscoverer GetActionDiscoverer();

        /// <summary>
        /// Gets the action invoker.
        /// </summary>
        /// <returns>Instance of <see cref="IActionInvoker"/></returns>
        IActionInvoker GetActionInvoker();

        /// <summary>
        /// Gets the JSON serializer.
        /// </summary>
        /// <returns>Instance of <see cref="IJsonSerializer"/></returns>
        IJsonSerializer GetJsonSerializer();

        /// <summary>
        /// Gets the actions validator.
        /// </summary>
        /// <returns>Instance of <see cref="IActionsValidator"/></returns>
        IActionsValidator GetActionsValidator();

        /// <summary>
        /// Gets the authorization policy store.
        /// </summary>
        /// <returns>Instance of <see cref="IAuthorizationPolicyStore"/></returns>
        IAuthorizationPolicyStore GetAuthorizationPolicyStore();

        /// <summary>
        /// Gets the controller builder.
        /// </summary>
        /// <returns>Instance of <see cref="IControllerBuilder"/></returns>
        IControllerBuilder GetControllerBuilder();

        /// <summary>
        /// Gets the controller discoverer.
        /// </summary>
        /// <returns>Instance of <see cref="IControllerDiscoverer"/></returns>
        IControllerDiscoverer GetControllerDiscoverer();

        /// <summary>
        /// Gets the controller validator.
        /// </summary>
        /// <returns>Instance of <see cref="IControllersValidator"/></returns>
        IControllersValidator GetControllerValidator();

        /// <summary>
        /// Gets the parameters discoverer.
        /// </summary>
        /// <returns>Instance of <see cref="IParametersDiscoverer"/></returns>
        IParametersDiscoverer GetParametersDiscoverer();

        /// <summary>
        /// Gets the path resolver.
        /// </summary>
        /// <returns>Instance of <see cref="IPathResolver"/></returns>
        IPathResolver GetPathResolver();

        /// <summary>
        /// Gets the parameters validator.
        /// </summary>
        /// <returns>Instance of <see cref="IParametersValidator"/></returns>
        IParametersValidator GetParametersValidator();
        
        /// <summary>
        /// Resolves the specified interface type.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns>Instance of provided type</returns>
        object Resolve(Type interfaceType);

        /// <summary>
        /// Resolves instance of T.
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>Instance of T</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// Registers or replaces service.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        void Register<TInterface, TService>() where TService : class;

        /// <summary>
        /// Registers or replaces the specified factory.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="factory">The factory.</param>
        void Register<TInterface>(Func<TInterface> factory);

        /// <summary>
        /// Registers or replaces singleton instance.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        void RegisterSingleton<TInterface, TService>() where TService : class;

        /// <summary>
        /// Registers or replaces single instance.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="instance">The instance to register.</param>
        void RegisterInstance<TInterface>(object instance);

        /// <summary>
        /// Determines whether provided service is registered.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <returns>
        ///   <c>true</c> if service is registered; otherwise, <c>false</c>.
        /// </returns>
        bool IsServiceRegistered<TInterface>();

        /// <summary>
        /// Determines whether service is registered.
        /// </summary>
        /// <param name="tInterface">The type of interface.</param>
        /// <returns><c>true</c> if service is registered; otherwise, <c>false</c>.</returns>
        bool IsServiceRegistered(Type tInterface);
    }
}
