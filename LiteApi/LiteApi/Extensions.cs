using System;
using System.Linq;
using System.Reflection;

namespace LiteApi
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Determines whether the type is nullable.
        /// </summary>
        /// <param name="info">The type information.</param>
        /// <param name="nullableArgument">The argument of nullable type.</param>
        /// <returns></returns>
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
