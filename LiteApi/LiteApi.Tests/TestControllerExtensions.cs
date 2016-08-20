using LiteApi.Contracts.Models;
using LiteApi.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LiteApi.Tests
{
    internal static class TestControllerExtensions
    {
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
    }
}
