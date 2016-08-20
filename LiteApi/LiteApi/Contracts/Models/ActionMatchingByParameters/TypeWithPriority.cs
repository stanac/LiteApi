using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LiteApi.Contracts.Models.ActionMatchingByParameters
{
    public class TypeWithPriority
    {
        public Type Type { get; private set; }
        public int TypePriority { get; private set; }

        public TypeWithPriority(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            Type = type;
            SetTypePriority();
        }

        public static int GetTypePriority(Type type)
        {
            var info = type.GetTypeInfo();
            if (info.IsGenericType)
            {
                type = info.GetGenericArguments().Single();
            }
            return new TypeWithPriority(type).TypePriority;
        }

        private void SetTypePriority()
        {
            if (Type == typeof(bool)) TypePriority = 10;
            else if (Type == typeof(Guid)) TypePriority = 20;
            else if (Type == typeof(decimal)) TypePriority = 30;
            else if (Type == typeof(float)) TypePriority = 40;
            else if (Type == typeof(double)) TypePriority = 50;
            else if (Type == typeof(SByte)) TypePriority = 55;
            else if (Type == typeof(Byte)) TypePriority = 56;
            else if (Type == typeof(UInt64)) TypePriority = 60;
            else if (Type == typeof(UInt32)) TypePriority = 70;
            else if (Type == typeof(UInt16)) TypePriority = 80;
            else if (Type == typeof(Int64)) TypePriority = 90;
            else if (Type == typeof(Int16)) TypePriority = 100;
            else if (Type == typeof(Int32)) TypePriority = 110;
            else if (Type == typeof(DateTime)) TypePriority = 120;
            else if (Type == typeof(char)) TypePriority = 200;
            else if (Type == typeof(string)) TypePriority = 250;
#pragma warning disable RECS0143 // Cannot resolve symbol in text argument
            else throw new ArgumentOutOfRangeException("Type", "Failed to set type priority");
#pragma warning restore RECS0143 // Cannot resolve symbol in text argument
        }
    }
}
