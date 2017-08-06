using LiteApi.Contracts.Abstractions;
using LiteApi.Contracts.Models;
using LiteApi.Services;
using LiteApi.Services.ModelBinders;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Reflection;
using Xunit;

namespace LiteApi.Tests
{
    public abstract class ParametersTestsBase
    {
        public abstract bool TestHeader { get; }
        
        protected void AssertParamParse(string paramKey, string[] paramValues, Type type, object expectedValue, bool hasDefaultValue = false, object defaultValue = null, string overrideParamName = null)
        {
            IModelBinder binder = GetModelBinder();
            HttpRequest request = GetHttpRequest(paramKey, paramValues);
            ActionContext action = GetActionContext(binder, type, hasDefaultValue, defaultValue, overrideParamName);

            object[] parameters = binder.GetParameterValues(request, action);

            Assert.Equal(1, parameters.Length);
            Assert.Equal(expectedValue, parameters[0]);
        }

        protected void AssertException(Action action, Func<Exception, bool> assert)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Assert.True(assert(ex));
                return;
            }
            Assert.True(false, "Exception is not thrown");
        }

        private IModelBinder GetModelBinder()
            => new ModelBinderCollection(new JsonSerializer(), Fakes.FakeServiceProvider.GetServiceProvider());

        private HttpRequest GetHttpRequest(string paramKey, params string[] paramValues)
        {
            var request = Fakes.FakeHttpRequest.WithGetMethod();
            if (paramKey != null)
            {
                if (TestHeader) request.Headers[paramKey] = paramValues;
                else request.AddQuery(paramKey, paramValues);
            }
            return request;
        }

        private ActionContext GetActionContext(IModelBinder modelBinders, Type paramType, bool hasDefaultValue = false, object defaultValue = null, string overrideParamName = null)
        {
            var action = new ActionContext
            {
                Filters = new ApiFilterWrapper[0],
                HttpMethod = SupportedHttpMethods.Get,
                Method = typeof(ParametersTestsBase).GetMethod("ActionMethod", BindingFlags.Instance | BindingFlags.NonPublic),
                RouteSegments = new RouteSegment[0],
                SkipAuth = true,
                ParentController = new ControllerContext
                {
                    RouteAndName = "asd",
                    ControllerType = typeof(ParametersTestsBase)
                },
                Parameters = new ActionParameter[1]
            };
            action.Parameters[0] = new ActionParameter(action, modelBinders)
            {
                Name = "input",
                ParameterSource = TestHeader ? ParameterSources.Header : ParameterSources.Query,
                Type = paramType,
                HasDefaultValue = hasDefaultValue,
                DefaultValue = defaultValue,
                OverridenName = overrideParamName
            };
            return action;
        }

        // in use for faking action context
        private object ActionMethod(object input) => input;

    }
}
