using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Services.Discoverers;
using LiteApi.Services.Validators;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiteApi.Services
{
    /// <summary>
    /// Default implementation of ILiteApiServiceResolver
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.ILiteApiServiceResolver" />
    public class LiteApiServiceResolver : ILiteApiServiceResolver
    {
        private IServiceProvider _serviceProvider;
        private readonly IDictionary<Type, ServiceRegistrationModel> _serviceRegs = new ConcurrentDictionary<Type, ServiceRegistrationModel>();

        /// <summary>
        /// Initializes the service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public virtual void Initialize(IServiceProvider serviceProvider, LiteApiOptions options)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            if (options == null) throw new ArgumentNullException(nameof(options));
            RegisterAllInternalServices(options);
        }
        
        private void RegisterAllInternalServices(LiteApiOptions options)
        {
            if (!IsServiceRegistered<IActionDiscoverer>())
                Register<IActionDiscoverer, ActionDiscoverer>();

            if (!IsServiceRegistered<IActionInvoker>())
                RegisterSingleton<IActionInvoker, ActionInvoker>();

            if (!IsServiceRegistered<IActionsValidator>())
                Register<IActionsValidator, ActionsValidator>();

            if (!IsServiceRegistered<IControllerBuilder>())
                Register<IControllerBuilder, ControllerBuilder>();

            if (!IsServiceRegistered<IControllerDiscoverer>())
                Register<IControllerDiscoverer, ControllerDiscoverer>();

            if (!IsServiceRegistered<IControllersValidator>())
                Register<IControllersValidator, ControllersValidator>();

            if (!IsServiceRegistered<IParametersDiscoverer>())
                Register<IParametersDiscoverer, ParametersDiscoverer>();

            if (!IsServiceRegistered<IParametersValidator>())
                Register<IParametersValidator, ParametersValidator>();

            if (!IsServiceRegistered<IJsonSerializer>())
                RegisterInstance<IJsonSerializer>(new JsonSerializer());

            if (!IsServiceRegistered<ILiteApiOptionsRetriever>())
                RegisterInstance<ILiteApiOptionsRetriever>(new LiteApiOptionsRetriever(options));
        }

        #region Get services

        /// <summary>
        /// Gets the action discoverer.
        /// </summary>
        /// <returns>
        /// Instance of <see cref="T:LiteApi.Contracts.Abstractions.IActionDiscoverer" />
        /// </returns>
        public virtual IActionDiscoverer GetActionDiscoverer() => Resolve<IActionDiscoverer>();

        /// <summary>
        /// Gets the action invoker.
        /// </summary>
        /// <returns>
        /// Instance of <see cref="T:LiteApi.Contracts.Abstractions.IActionInvoker" />
        /// </returns>
        public virtual IActionInvoker GetActionInvoker() => Resolve<IActionInvoker>();

        /// <summary>
        /// Gets the JSON serializer.
        /// </summary>
        /// <returns>Instance of <see cref="IJsonSerializer"/></returns>
        public virtual IJsonSerializer GetJsonSerializer() => Resolve<IJsonSerializer>();

        /// <summary>
        /// Gets the actions validator.
        /// </summary>
        /// <returns>
        /// Instance of <see cref="T:LiteApi.Contracts.Abstractions.IActionsValidator" />
        /// </returns>
        public virtual IActionsValidator GetActionsValidator() => Resolve<IActionsValidator>();

        /// <summary>
        /// Gets the authorization policy store.
        /// </summary>
        /// <returns>
        /// Instance of <see cref="T:LiteApi.Contracts.Abstractions.IAuthorizationPolicyStore" />
        /// </returns>
        public virtual IAuthorizationPolicyStore GetAuthorizationPolicyStore() => Resolve<IAuthorizationPolicyStore>();

        /// <summary>
        /// Gets the controller builder.
        /// </summary>
        /// <returns>
        /// Instance of <see cref="T:LiteApi.Contracts.Abstractions.IControllerBuilder" />
        /// </returns>
        public virtual IControllerBuilder GetControllerBuilder() => Resolve<IControllerBuilder>();

        /// <summary>
        /// Gets the controller discoverer.
        /// </summary>
        /// <returns>
        /// Instance of <see cref="T:LiteApi.Contracts.Abstractions.IControllerDiscoverer" />
        /// </returns>
        public virtual IControllerDiscoverer GetControllerDiscoverer() => Resolve<IControllerDiscoverer>();

        /// <summary>
        /// Gets the controller validator.
        /// </summary>
        /// <returns>
        /// Instance of <see cref="T:LiteApi.Contracts.Abstractions.IControllersValidator" />
        /// </returns>
        public virtual IControllersValidator GetControllerValidator() => Resolve<IControllersValidator>();

        /// <summary>
        /// Gets the parameters discoverer.
        /// </summary>
        /// <returns>
        /// Instance of <see cref="T:LiteApi.Contracts.Abstractions.IParametersDiscoverer" />
        /// </returns>
        public virtual IParametersDiscoverer GetParametersDiscoverer() => Resolve<IParametersDiscoverer>();

        /// <summary>
        /// Gets the path resolver.
        /// </summary>
        /// <returns>
        /// Instance of <see cref="T:LiteApi.Contracts.Abstractions.IPathResolver" />
        /// </returns>
        public virtual IPathResolver GetPathResolver() => Resolve<IPathResolver>();

        /// <summary>
        /// Gets the parameters validator.
        /// </summary>
        /// <returns>Instance of <see cref="IParametersValidator"/></returns>
        public virtual IParametersValidator GetParametersValidator() => Resolve<IParametersValidator>();

        /// <summary>
        /// Gets the middleware options retriever.
        /// </summary>
        /// <returns>Instance of <see cref="ILiteApiOptionsRetriever"/></returns>
        public virtual ILiteApiOptionsRetriever GetOptionsRetriever() => Resolve<ILiteApiOptionsRetriever>();

        #endregion

        #region Register

        /// <summary>
        /// Registers service instance.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        public virtual void Register<TInterface, TService>()
                    where TService : class
        {
            _serviceRegs[typeof(TInterface)] = new ServiceRegistrationModel(typeof(TInterface), typeof(TService), false);
        }

        /// <summary>
        /// Registers singleton instance.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TService">The type of the service.</typeparam>
        public virtual void RegisterSingleton<TInterface, TService>()
                    where TService : class
        {
            _serviceRegs[typeof(TInterface)] = new ServiceRegistrationModel(typeof(TInterface), typeof(TService), true);
        }

        /// <summary>
        /// Registers single instance.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="instance">The instance to register.</param>
        public virtual void RegisterInstance<TInterface>(object instance)
        {
            _serviceRegs[typeof(TInterface)] = new ServiceRegistrationModel(typeof(TInterface), instance);
        }
        
        /// <summary>
        /// Registers the specified factory.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <param name="factory">The factory.</param>
        public virtual void Register<TInterface>(Func<TInterface> factory)
        {
            _serviceRegs[typeof(TInterface)] = new ServiceRegistrationModel(typeof(TInterface), factory);
        }

        #endregion

        #region Resolve

        /// <summary>
        /// Resolves typeof(T) instance.
        /// </summary>
        /// <typeparam name="T">Type to resolve</typeparam>
        /// <returns>Instance of T</returns>
        public virtual T Resolve<T>()
                    where T : class
                    => Resolve(typeof(T)) as T;

        /// <summary>
        /// Resolves the specified interface type.
        /// </summary>
        /// <param name="interfaceType">Type of the interface.</param>
        /// <returns>Instance of requested interface type.</returns>
        public virtual object Resolve(Type interfaceType)
        {
            if (_serviceRegs.ContainsKey(interfaceType))
            {
                var reg = _serviceRegs[interfaceType];
                if (reg != null)
                {
                    return reg.GetService(this);
                }
            }

            var obj = _serviceProvider.GetService(interfaceType);
            if (obj != null)
            {
                return obj;
            }

            throw new ArgumentException($"Service of type {interfaceType} is not registered in LiteApiServiceResolver.");
        }

        #endregion

        #region IsServiceRegistered

        /// <summary>
        /// Determines whether service is registered.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <returns>
        ///   <c>true</c> if service is registered; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsServiceRegistered<TInterface>() => _serviceRegs.ContainsKey(typeof(TInterface));

        /// <summary>
        /// Determines whether service is registered.
        /// </summary>
        /// <param name="tInterface">The type of interface.</param>
        /// <returns><c>true</c> if service is registered; otherwise, <c>false</c>.</returns>
        public virtual bool IsServiceRegistered(Type tInterface) => _serviceRegs.ContainsKey(tInterface);

        #endregion

        private class ServiceRegistrationModel
        {
            public Type InterfaceType { get; private set; }
            public Type ImplementationType { get; private set; }
            public bool IsSingleton { get; private set; }
            public object SingletonInstance { get; private set; }
            public Func<object> Factory { get; private set; }

            private ConstructorInfo _constructorInfo;
            private Type[] _constructorParamTypes;

            public ServiceRegistrationModel(Type tInterface, Func<object> factory)
            {
                InterfaceType = tInterface ?? throw new ArgumentNullException(nameof(tInterface));
                Factory = factory ?? throw new ArgumentNullException(nameof(factory));
            }

            public ServiceRegistrationModel(Type tInterface, object instance)
            {
                InterfaceType = tInterface ?? throw new ArgumentNullException(nameof(tInterface));
                SingletonInstance = instance ?? throw new ArgumentNullException(nameof(instance));
                ImplementationType = instance.GetType();
                if (!tInterface.IsAssignableFrom(ImplementationType)) throw new Exception(
                    $"Failed to register internal service. Type {ImplementationType} is not assignable from {tInterface}."
                    );
                IsSingleton = true;
            }

            public ServiceRegistrationModel(Type tInterface, Type tImplementation, bool isSingleton)
            {
                if (!tInterface.IsAssignableFrom(tImplementation)) throw new Exception(
                    $"Failed to register internal service. Type {tImplementation} is not assignable from {tInterface}."
                    );
                if (!tInterface.GetTypeInfo().IsInterface) throw new Exception(
                    $"Failed to register internal service. Type {tInterface} is not an interface."
                    );
                IsSingleton = isSingleton;
                ImplementationType = tImplementation;
                InterfaceType = tInterface;
                var constructors = tImplementation.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
                if (constructors.Length > 1)
                {
                    constructors = constructors.Where(x => x.GetCustomAttribute<PrimaryConstructorAttribute>() != null).ToArray();
                }
                if (constructors.Length != 1) throw new Exception(
                    $"Failed to register interface {tInterface}, {tImplementation} does not have public constructor or it has more than one constructor."
                    + $"Use {typeof(PrimaryConstructorAttribute)} to mark only one constructor as primary if there is more than one."
                    );

                _constructorInfo = constructors[0];
                _constructorParamTypes = _constructorInfo.GetParameters().Select(x => x.ParameterType).ToArray();
            }

            public object GetService(LiteApiServiceResolver serviceResolver)
            {
                if (Factory != null) return Factory();

                if (IsSingleton)
                {
                    if (SingletonInstance == null)
                    {
                        SingletonInstance = BuildObject(serviceResolver);
                    }
                    return SingletonInstance;
                }

                return BuildObject(serviceResolver);
            }

            public object BuildObject(LiteApiServiceResolver serviceResolver)
            {
                var parameters = new object[_constructorParamTypes.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    parameters[i] = serviceResolver.Resolve(_constructorParamTypes[i]);
                }
                return _constructorInfo.Invoke(parameters);
            }
        }
    }
}
