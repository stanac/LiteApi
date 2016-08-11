using LiteApi.Attributes;
using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiteApi.Services
{
    public class ControllerBuilder : IControllerBuilder
    {
        private static readonly Dictionary<string, ConstructorInfo> Constructors = new Dictionary<string, ConstructorInfo>();
        private static readonly Dictionary<string, ParameterInfo[]> ConstructorParameterTypes = new Dictionary<string, ParameterInfo[]>();
        // private static readonly Dictionary<string, LiteController> SingletonControllers = new Dictionary<string, LiteController>();
        private static readonly CompiledActionInvokerCache _cache = new CompiledActionInvokerCache();
        // private static readonly object Sync = new object();

        public LiteController Build(ControllerContext controllerCtx, HttpContext httpContext)
        {
            //if (controllerCtx.IsSingleton)
            //{
            //    string key = controllerCtx.ControllerType.FullName;
            //    LiteController ctrl = null;
            //    if (!SingletonControllers.ContainsKey(key))
            //    {
            //        lock (Sync)
            //        {
            //            if (!SingletonControllers.ContainsKey(key))
            //            {
            //                ConstructorInfo constructor = GetConstructor(controllerCtx.ControllerType);
            //                ParameterInfo[] parameters = GetConstructorParameters(constructor);
            //                object[] parameterValues = GetConstructorParameterValues(parameters);
            //                var controller = constructor.Invoke(parameterValues) as LiteController;
            //                controller.IsSingleton = true;
            //                // DO NOT SET HttpContext on singleton controller
            //                SingletonControllers[key] = controller;
            //            }
            //        }
            //    }
            //    return SingletonControllers[key];
            //}
            //else
            //{
                ConstructorInfo constructor = GetConstructor(controllerCtx.ControllerType);
                ParameterInfo[] parameters = GetConstructorParameters(constructor);
                object[] parameterValues = GetConstructorParameterValues(parameters);
                // var controller = _cache.GetProxy(controllerCtx.ControllerGuid).InvokeConstructor(parameterValues) as LiteController;
                var controller = constructor.Invoke(parameterValues) as LiteController;
                controller.HttpContext = httpContext;
                return controller;
            //}
        }

        internal static ConstructorInfo GetConstructor(Type controllerType)
        {
            if (Constructors.ContainsKey(controllerType.FullName))
            {
                return Constructors[controllerType.FullName];
            }

            var constructors = controllerType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length > 1)
            {
                constructors = constructors.Where(x => x.GetCustomAttribute<ApiConstructorAttribute>() != null).ToArray();
            }

            if (constructors.Length != 1)
            {
                throw new Exception($"Cannot find constructor for {controllerType.FullName}. Class has more than one constructor, or "
                    + "more than one constructor is using ApiConstructorAttribute. If class has more than one constructor, only "
                    + "one should be annotated with ApiConstructorAttribute.");
            }

            Constructors[controllerType.FullName] = constructors[0];
            return constructors[0];
        }

        private static ParameterInfo[] GetConstructorParameters(ConstructorInfo constructor)
        {
            if (ConstructorParameterTypes.ContainsKey(constructor.DeclaringType.FullName))
            {
                return ConstructorParameterTypes[constructor.DeclaringType.FullName];
            }

            ParameterInfo[] parameters = constructor.GetParameters();
            ConstructorParameterTypes[constructor.DeclaringType.FullName] = parameters;
            return parameters;
        }

        private static object[] GetConstructorParameterValues(ParameterInfo[] parameters)
        {
            object[] values = new object[parameters.Length];
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = LiteApiMiddleware.Services.GetService(parameters[i].ParameterType);
            }
            return values;
        }
    }
}
