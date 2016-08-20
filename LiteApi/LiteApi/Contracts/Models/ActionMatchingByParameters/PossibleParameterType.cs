using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Reflection;

namespace LiteApi.Contracts.Models.ActionMatchingByParameters
{
    public class PossibleParameterType
    {
        public string Name { get; set; }
        public int OrderId { get; set; }
        public ParameterSources Source { get; set; }
        public TypeWithPriority[] PossibleTypes { get; set; } = new TypeWithPriority[0];
        public StringValues QueryValues { get; set; }
        public bool HasValue { get; set; }

        public Type GetMatchingType(Type type)
        {
            var info = type.GetTypeInfo();
            if (!info.IsGenericType)
            {
                return type;
            }
            return info.GenericTypeArguments.Single();
        }
    }
}
