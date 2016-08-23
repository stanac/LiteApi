using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Reflection;

namespace LiteApi.Contracts.Models.ActionMatchingByParameters
{
    /// <summary>
    /// Possible type match to a query value
    /// </summary>
    public class PossibleParameterType
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        public int OrderId { get; set; }
        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public ParameterSources Source { get; set; }
        /// <summary>
        /// Gets or sets the possible types.
        /// </summary>
        /// <value>
        /// The possible types.
        /// </value>
        public TypeWithPriority[] PossibleTypes { get; set; } = new TypeWithPriority[0];
        /// <summary>
        /// Gets or sets the query values.
        /// </summary>
        /// <value>
        /// The query values.
        /// </value>
        public StringValues QueryValues { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has value.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has value; otherwise, <c>false</c>.
        /// </value>
        public bool HasValue { get; set; }

        /// <summary>
        /// Determines whether this instance can handle the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public bool CanHandleType(Type type)
        {
            type = GetNotNullableType(type);
            return PossibleTypes.Any(x => x.Type == type);
        }

        /// <summary>
        /// Gets the not nullable type of provided type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Not nullable type</returns>
        public Type GetNotNullableType(Type type)
        {
            var info = type.GetTypeInfo();
            if (!info.IsGenericType)
            {
                return type;
            }
            return info.GenericTypeArguments.FirstOrDefault();
        }
    }
}
