using LiteApi.Services.ModelBinders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LiteApi.Contracts.Models;
using Microsoft.AspNetCore.Http;
using LiteApi.Contracts.Abstractions;

namespace LiteApi.Demo
{
    public class StackQueryBinder: BasicQueryModelBinder
    {
        public StackQueryBinder(ILiteApiOptionsAccessor optionsRetriever)
            : base(optionsRetriever) { }

        public override bool DoesSupportType(Type type)
        {
            var info = type.GetTypeInfo();
            return 
                info.IsGenericType 
                && info.GetGenericTypeDefinition() == typeof(Stack<>)
                && base.DoesSupportType(info.GetGenericArguments()[0]);
        }

        public override object ParseParameterValue(HttpRequest request, ActionContext actionCtx, ActionParameter parameter)
        {
            string queryKey = request.Query.First(x => 
                string.Compare(x.Key, parameter.Name, StringComparison.OrdinalIgnoreCase) == 0
                ).Key;
            // using queryKey to match in case insensitive manner
            string[] values = request.Query[queryKey];

            object stack = Activator.CreateInstance(parameter.Type);
            if (values.Any())
            {
                var details = new StackParameterDetails(parameter.Type.GetGenericArguments()[0]);
                var pushMethod = parameter.Type.GetMethod("Push", BindingFlags.Public | BindingFlags.Instance);
                foreach (var queryValue in values)
                {
                    object value = ParseSingleQueryValue(
                        queryValue, 
                        details.BaseType, // needs to be base type, e.g. if parameter type is int? we need to pass int
                        details.IsNullable, 
                        parameter.Name, 
                        new Lazy<string>(() => actionCtx.Name),
                        request.HttpContext
                        );
                    pushMethod.Invoke(stack, new[] { value });
                }
            }
            return stack;
        }

        private class StackParameterDetails
        {
            // TODO: cache result to improve performance
            public StackParameterDetails(Type type)
            {
                Type baseType;
                
                if (type.IsNullable(out baseType)) // extension method from LiteApi namespace
                {
                    BaseType = baseType;
                    IsNullable = true;
                }
                else
                {
                    BaseType = type;
                }
            }

            public Type BaseType { get; set; }
            public bool IsNullable { get; set; }
        }
    }
}
