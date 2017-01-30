using System;
using System.Linq;
using System.Reflection;

namespace LiteApi.Contracts.Models.ActionMatchingByParameters
{
    /// <summary>
    /// Determines type priority for action overloading
    /// </summary>
    public class TypeWithPriority
    {
        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; private set; }

        /// <summary>
        /// Gets the type priority.
        /// </summary>
        /// <value>
        /// The type priority.
        /// </value>
        public int TypePriority { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeWithPriority" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public TypeWithPriority(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            Type = type;
            SetTypePriority();
        }

        /// <summary>
        /// Gets the type priority.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>int, Types priority</returns>
        public static int GetTypePriority(Type type)
        {
            var info = type.GetTypeInfo();
            if (info.IsGenericType)
            {
                type = info.GetGenericArguments().Single();
            }
            return new TypeWithPriority(type).TypePriority;
        }

        /// <summary>
        /// Sets the type priority.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">Type;Failed to set type priority</exception>
        private void SetTypePriority()
        {
            // todo: make switch
            if (Type == typeof(bool)) TypePriority = 10;
            else if (Type == typeof(Guid)) TypePriority = 20;
            else if (Type == typeof(float)) TypePriority = 30;
            else if (Type == typeof(decimal)) TypePriority = 40;
            else if (Type == typeof(double)) TypePriority = 50;
            else if (Type == typeof(Byte)) TypePriority = 53;
            else if (Type == typeof(SByte)) TypePriority = 56;
            else if (Type == typeof(UInt16)) TypePriority = 60;
            else if (Type == typeof(UInt32)) TypePriority = 70;
            else if (Type == typeof(UInt64)) TypePriority = 80;
            else if (Type == typeof(Int16)) TypePriority = 90;
            else if (Type == typeof(Int32)) TypePriority = 100;
            else if (Type == typeof(Int64)) TypePriority = 110;
            else if (Type == typeof(DateTime)) TypePriority = 120;
            else if (Type == typeof(char)) TypePriority = 200;
            else if (Type == typeof(string)) TypePriority = 250;
#pragma warning disable RECS0143 // Cannot resolve symbol in text argument
            else throw new ArgumentOutOfRangeException("Type", "Failed to set type priority");
#pragma warning restore RECS0143 // Cannot resolve symbol in text argument
        }
    }
}
