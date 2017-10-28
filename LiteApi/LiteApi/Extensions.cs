using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
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
        private static readonly string[] _commonNamespaces = { "System", "System.Collections.Generic", "System.Threading.Tasks" };

        /// <summary>
        /// Determines whether the type is nullable.
        /// </summary>
        /// <param name="info">The type information.</param>
        /// <param name="nullableArgument">The argument of nullable type.</param>
        /// <returns>True if type is nullable as in int? or Nullable{int}, otherwise false</returns>
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
        /// Determines whether the specified nullable argument is nullable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="nullableArgument">The nullable argument.</param>
        /// <returns>>True if type is nullable as in int? or Nullable{int}, otherwise false</returns>
        public static bool IsNullable(this Type type, out Type nullableArgument)
        {
            nullableArgument = null;
            var info = type.GetTypeInfo();
            bool isNullable = info.IsGenericType && info.GetGenericTypeDefinition() == typeof(Nullable<>);
            if (isNullable)
            {
                nullableArgument = info.GetGenericArguments().Single();
            }
            return isNullable;
        }

        /// <summary>
        /// Determines whether type is supported collection.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="collectionElement">The collection element type.</param>
        /// <returns>
        ///   <c>true</c> if type is supported collection type; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSupportedCollection(this Type type, out Type collectionElement)
            => IsSupportedCollection(type.GetTypeInfo(), out collectionElement);

        public static bool IsSupportedCollection(this TypeInfo type, out Type collectionElement)
        {
            collectionElement = null;
            if (typeof(IEnumerable).IsAssignableFrom(type.AsType()))
            {
                if (type.IsArray && type.GetArrayRank() == 1)
                {
                    collectionElement = type.GetElementType();
                    return true;
                }
                Type[] supportedCollections = { typeof(List<>), typeof(IEnumerable<>) };
                if (type.IsGenericType && supportedCollections.Contains(type.GetGenericTypeDefinition()))
                {
                    collectionElement = type.GetGenericArguments().Single();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determines whether request has body.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>True if request has body, otherwise false</returns>
        public static bool HasBody(this HttpRequest request)
        {
            return request.ContentLength.HasValue && request.ContentLength > 0;
        }

        /// <summary>
        /// Gets custom attributes where T is assignable to the attribute and cast them to T.
        /// </summary>
        /// <typeparam name="T">They to check and cast</typeparam>
        /// <param name="mi">Member info.</param>
        /// <returns>Returns casted attributes from member info</returns>
        public static IEnumerable<T> GetAttributesAs<T>(this MemberInfo mi)
        {
            return mi
                .GetCustomAttributes()
                .Where(x => typeof(T).IsAssignableFrom(x.GetType()))
                .Cast<T>();
        }

        /// <summary>
        /// Gets the name of the type as if it was written in code. (Useful for debugging)
        /// </summary>
        /// <param name="type">The type for which to get name</param>
        /// <param name="returnTypeFullName">Whether to return type full name or not, default is <see cref="TypeFullName.FullNameForUncommonTypes"/></param>
        /// <returns></returns>
        public static string GetFriendlyName(this Type type, TypeFullName returnTypeFullName = TypeFullName.FullNameForUncommonTypes)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            if (type.IsArray) return type.GetElementType().GetFriendlyName(returnTypeFullName) + "[]";

            var info = type.GetTypeInfo();

            if (!info.IsGenericType)
            {
                return GetTypeName(type, returnTypeFullName);
            }

            // type should be generic

            if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return $"{type.GetGenericArguments().First().GetFriendlyName(returnTypeFullName)}?";
            }

            string name = GetTypeName(type, returnTypeFullName);
            int index = name.IndexOf('`');
            name = name.Substring(0, index) + "<";
            var args = type.GetGenericArguments();
            for (int i = 0; i < args.Length; i++)
            {
                name += args[i].GetFriendlyName(returnTypeFullName);
                if (i < args.Length - 1)
                {
                    name += ", ";
                }
            }
            name += ">";
            return name;
        }

        private static string GetTypeName(Type type, TypeFullName returnTypeFullName)
        {
            string name = type.Name;
            if (returnTypeFullName == TypeFullName.FullName)
            {
                name = $"{type.Namespace}.{type.Name}";
            }
            else if (returnTypeFullName == TypeFullName.FullNameForUncommonTypes)
            {
                if (!_commonNamespaces.Contains(type.Namespace))
                {
                    name = $"{type.Namespace}.{type.Name}";
                }
            }
            // TypeFullName.ShortName is left
            return name;
        }
    }
}
