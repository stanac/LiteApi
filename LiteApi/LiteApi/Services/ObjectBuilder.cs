using LiteApi.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiteApi.Services
{
    /// <summary>
    /// Generic object instance builder that is using registered services withing the ASP.NET app
    /// </summary>
    public class ObjectBuilder
    {
        private static readonly IDictionary<string, ConstructorInfo> Constructors = new ConcurrentDictionary<string, ConstructorInfo>();
        private static readonly IDictionary<string, ParameterInfo[]> ConstructorParameterTypes = new ConcurrentDictionary<string, ParameterInfo[]>();

        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectBuilder"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public ObjectBuilder(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Builds the object.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>Object instance</returns>
        public virtual object BuildObject(Type objectType)
        {
            ConstructorInfo constructor = GetConstructor(objectType);
            ParameterInfo[] parameters = GetConstructorParameters(constructor);
            object[] parameterValues = GetConstructorParameterValues(parameters);
            object objectInstance = constructor.Invoke(parameterValues);
            return objectInstance;
        }

        /// <summary>
        /// Builds the object.
        /// </summary>
        /// <typeparam name="T">Type to build</typeparam>
        /// <returns>Instance of T</returns>
        public virtual T BuildObject<T>()
            where T: class
        {
            return BuildObject(typeof(T)) as T;
        }

        /// <summary>
        /// Gets the constructor.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>ConstructorInfo to use when constructing object</returns>
        /// <exception cref="System.ArgumentNullException">objectType - Type is not provided</exception>
        /// <exception cref="System.Exception"></exception>
        protected virtual ConstructorInfo GetConstructor(Type objectType)
        {
            if (objectType == null) throw new ArgumentNullException(nameof(objectType), "Type is not provided");

            if (Constructors.ContainsKey(objectType.FullName))
            {
                return Constructors[objectType.FullName];
            }

            var constructors = objectType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length > 1)
            {
                constructors = constructors.Where(x => x.GetCustomAttribute<PrimaryConstructorAttribute>() != null).ToArray();
            }

            if (constructors.Length != 1)
            {
                throw new Exception($"Cannot find constructor for {objectType.FullName}. Class has more than one constructor, or "
                    + "more than one constructor is using ApiConstructorAttribute. If class has more than one constructor, only "
                    + "one should be annotated with ApiConstructorAttribute.");
            }

            Constructors[objectType.FullName] = constructors[0];
            return constructors[0];
        }

        /// <summary>
        /// Gets the info about constructor parameters.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <returns>ParameterInfo[] for the given constructor</returns>
        protected virtual ParameterInfo[] GetConstructorParameters(ConstructorInfo constructor)
        {
            if (ConstructorParameterTypes.ContainsKey(constructor.DeclaringType.FullName))
            {
                return ConstructorParameterTypes[constructor.DeclaringType.FullName];
            }

            ParameterInfo[] parameters = constructor.GetParameters();
            ConstructorParameterTypes[constructor.DeclaringType.FullName] = parameters;
            return parameters;
        }

        /// <summary>
        /// Gets the constructor parameter values.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>object[] to pass to the constructor</returns>
        protected virtual object[] GetConstructorParameterValues(ParameterInfo[] parameters)
        {
            object[] values = new object[parameters.Length];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = _serviceProvider.GetService(parameters[i].ParameterType);
            }
            return values;
        }
    }
}