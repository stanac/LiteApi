using System;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Internal service resolver used to register and replace LiteApi internal services
    /// </summary>
    public interface ILiteApiServiceResolver
    {
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
        /// Replaces the service.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="singleton">if set to <c>true</c> will be registered as singleton.</param>
        void ReplaceService<TInterface, TService>(bool singleton) where TService : class;

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
        /// Registers service instance.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        void Register<TInterface, TService>() where TService : class;
        
        /// <summary>
        /// Registers singleton instance.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        void RegisterSingleton<TInterface, TService>() where TService : class;

        /// <summary>
        /// Registers single instance.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="instance">The instance to register.</param>
        void RegisterInstance<TInterface>(object instance);
        
    }
}
