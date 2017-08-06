﻿using LiteApi.Services;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace LiteApi.Tests
{
    public class RouteParametersInvokeTests
    {
        [Fact]
        public async Task Action_WithRouteParameterOfTypebool_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<bool>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypestring_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<string>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypechar_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<char>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypeInt16_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<Int16>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypeInt32_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<Int32>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypeInt64_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<Int64>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypeUInt16_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<UInt16>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypeUInt32_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<UInt32>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypeUInt64_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<UInt64>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypeByte_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<Byte>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypeSByte_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<SByte>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypedecimal_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<decimal>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypefloat_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<float>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypedouble_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<double>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypeDateTime_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<DateTime>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypeGuid_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<Guid>();
        }

        [Fact]
        public async Task Action_WithRouteParameterOfTypeDateOnly_CanBeInvoked()
        {
            await AssertCanInvokeActionWithRouteParamOfType<DateTime>(true);
        }

        [Fact]
        public async Task Action_WithRouteParametersWithCapitalLetters_CanBeInvoked()
        {
            var discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.RouteParamsController));
            var ctrl = discoverer.GetControllers(null).Single();
            var action = ctrl.Actions.Single(x => x.Name == "plus3");
            var serializer = new JsonSerializer();
            var invoker = new ActionInvoker(new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object), new Services.ModelBinders.ModelBinderCollection(serializer, Fakes.FakeServiceProvider.GetServiceProvider()), new JsonSerializer());
            var httpCtx = new Fakes.FakeHttpContext();
            httpCtx.Request.Path = "/api/v2/route/2/Plus3/4";
            await invoker.Invoke(httpCtx, action);
            string jsonResult = httpCtx.Response.ReadBody();
            object result = serializer.Deserialize(jsonResult, typeof(int));
            Assert.Equal(6, result);
        }

        private async Task AssertCanInvokeActionWithRouteParamOfType<T>(bool dateOnly = false)
        {
            Type type = typeof(T);
            
            object value = "";
            if (typeof(T) != typeof(string))
            {
                value = Activator.CreateInstance(type);
            }
            if (typeof(T) == typeof(DateTime))
            {
                value = default(DateTime).ToString("yyyy-MM-dd HH:mm:ss");
                if (dateOnly)
                {
                    value = default(DateTime).ToString("yyyy-MM-dd");
                }
            }

            string actionName = "Action_" + type.Name;

            var discoverer = new Fakes.FakeLimitedControllerDiscoverer(typeof(Controllers.RouteSupportedParametersController));
            var ctrl = discoverer.GetControllers(null).Single();
            var action = ctrl.Actions.Single(x => x.Name == actionName.ToLower());
            var serializer = new JsonSerializer();
            var invoker = new ActionInvoker(new ControllerBuilder((new Moq.Mock<IServiceProvider>()).Object), new Services.ModelBinders.ModelBinderCollection(serializer, Fakes.FakeServiceProvider.GetServiceProvider()), new JsonSerializer());
            var httpCtx = new Fakes.FakeHttpContext();
            httpCtx.Request.Path = "/api/RouteSupportedParameters/" + actionName + "/" + value;
            await invoker.Invoke(httpCtx, action);
            string jsonResult = httpCtx.Response.ReadBody();
            object result = serializer.Deserialize(jsonResult, type);
            if (typeof(T) == typeof(DateTime))
            {
                value = default(DateTime);
            }
            Assert.Equal(value, result);
        }
    }
}
