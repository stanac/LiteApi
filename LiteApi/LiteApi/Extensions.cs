using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Gets custom attributes where T is assignable to the attribute and cast them to T.
        /// </summary>
        /// <typeparam name="T">They to check and cast</typeparam>
        /// <param name="mi">Member info.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetAttributesAs<T>(this MemberInfo mi)
        {
            return mi
                .GetCustomAttributes()
                .Where(x => typeof(T).IsAssignableFrom(x.GetType()))
                .Cast<T>();
        }
    }
}
