using LiteApi.Attributes;
using LiteApi.Contracts;
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

        public LiteController Build(ControllerContext controllerCtx)
        {
            ConstructorInfo constructor = GetConstructor(controllerCtx.ControllerType);
            ParameterInfo[] parameters = GetConstructorParameters(constructor);
            object[] parameterValues = GetConstructorParameterValues(parameters);
            return constructor.Invoke(parameterValues) as LiteController;
        }

        private static ConstructorInfo GetConstructor(Type controllerType)
        {
            if (Constructors.ContainsKey(controllerType.FullName))
            {
                return Constructors[controllerType.FullName];
            }

            var constructors = controllerType.GetConstructors(BindingFlags.Public);
            if (constructors.Length > 1)
            {
                constructors = constructors.Where(x => x.GetCustomAttribute<ApiConstructorAttribute>() != null).ToArray();
            }

            if (constructors.Length != 1)
            {
                throw new Exception($"Cannot find constructor for {controllerType.FullName}. Class more than one constructor, or "
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
