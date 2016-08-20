using LiteApi.Contracts.Models;
using LiteApi.Services;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Reflection;
using Xunit;

namespace LiteApi.Tests
{
    internal static class TestExtensions
    {
        public static void ResolveAndAssert(this PathResolver resolver, HttpRequest request, string expectedResult)
        {
            var action = resolver.ResolveAction(request);
            string actualResult = action.InvokeStringMethod();
            Assert.Equal(expectedResult, actualResult);
        }

        public static string InvokeStringMethod(this ActionContext action)
        {
            var parentObj = action.ParentController.ControllerType.GetConstructor(new Type[0]).Invoke(new object[0]);
            var parameters = action.Method.GetParameters().Select(x => x.ParameterType.GetDefaultValue()).ToArray();
            return action.Method.Invoke(parentObj, parameters) as string;
        }

        public static ControllerContext GetControllerContext(this LiteController ctrl)
        {
            var type = ctrl.GetType();
            var context = new ControllerContext
            {
                ControllerType = type,
                Name = GetControllerName(type),
                UrlRoot = "api"
            };
            var ad = new ActionDiscoverer(new ParametersDiscoverer());
            context.Actions = ad.GetActions(context);

            return context;
        }

        public static ControllerContext[] GetControllerContextAsArray(this LiteController ctrl)
        {
            return new ControllerContext[] { ctrl.GetControllerContext() };
        }

        private static string GetControllerName(Type type)
        {
            var method = typeof(ControllerDiscoverer).GetMethod("GetControllerName", BindingFlags.NonPublic | BindingFlags.Static);
            return method.Invoke(null, new object[] { type.FullName }) as string;
        }

        private static object GetDefaultValue(this Type type)
        {
            if (type.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}
