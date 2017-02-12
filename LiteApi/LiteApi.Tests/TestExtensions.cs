using LiteApi.Contracts.Models;
using LiteApi.Services;
using LiteApi.Services.Discoverers;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
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
                RouteAndName = GetControllerName(type)
            };
            var ad = new ActionDiscoverer(new ParametersDiscoverer(new Moq.Mock<IServiceProvider>().Object));
            context.Actions = ad.GetActions(context);

            return context;
        }

        public static ControllerContext[] GetControllerContextAsArray(this LiteController ctrl)
        {
            return new ControllerContext[] { ctrl.GetControllerContext() };
        }

        private static string GetControllerName(Type type)
        {
            var method = typeof(ControllerDiscoverer).GetMethod("GetControllerRute", BindingFlags.NonPublic | BindingFlags.Static);
            return method.Invoke(null, new object[] { type }) as string;
        }

        private static object GetDefaultValue(this Type type)
        {
            if (type.GetTypeInfo().IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public static string ReadBody(this HttpResponse response)
        {
            if (response.Body == null) return null;
            if (response.Body.Length == 0) return "";
            long position = response.Body.Position;
            response.Body.Position = 0;
            byte[] data = new byte[response.Body.Length];
            response.Body.Read(data, 0, data.Length);
            response.Body.Position = position;
            return Encoding.UTF8.GetString(data);
        }

        public static void AssertExpectedException<T>(Action action, string errorMessage)
            where T: Exception
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (!typeof(T).IsAssignableFrom(ex.GetType()))
                {
                    Assert.True(false, $"Exception of type {typeof(T).FullName} was expected but not thrown, instead exception of type {ex.GetType().FullName} was thrown, details: {errorMessage}, exception details {ex}");
                }

                Assert.True(true);
                return;
            }

            Assert.True(false, $"Exception of type {typeof(T).FullName} was expected but not thrown, details: {errorMessage}");
        }
    }
}
