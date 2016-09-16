using System;
using System.Linq;
using System.Reflection;

namespace LiteApi
{
    public static class Extensions
    {
        public static bool IsNullable(this TypeInfo info, out Type nullableArgument)
        {
            nullableArgument = null;
            bool isNullable = info.IsGenericType && info.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (isNullable)
            {
                nullableArgument = info.GetGenericArguments().Single();
            }
            return isNullable;
        }
    }
}
