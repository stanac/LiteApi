using System;
using System.Collections.Generic;
using System.Text;

namespace LiteApi.OpenApi.Models
{
    static class PrimitiveTypeConverter
    {
        public static bool IsTypePrimitive(Type t)
        {
            string name, format;
            GetTypeNameAndFormat(t, out name, out format);
            return name != null;
        }

        public static void GetTypeNameAndFormat(Type type, out string name, out string format)
        {
            name = null;
            format = null;

            if (type == typeof(string))
            {
                name = "string";
            }
            if (type == typeof(char))
            {
                name = "string";
                format = "char";
            }
            if (type == typeof(Boolean))
            {
                name = "boolean";
            }
            else if (type == typeof(int))
            {
                name = "integer";
                format = "int32";
            }
            else if (type == typeof(long))
            {
                name = "integer";
                format = "int64";
            }
            else if (type == typeof(short))
            {
                name = "integer";
                format = "int16";
            }
            else if (type == typeof(SByte))
            {
                name = "integer";
                format = "int8";
            }
            else if (type == typeof(uint))
            {
                name = "integer";
                format = "int32";
            }
            else if (type == typeof(ulong))
            {
                name = "integer";
                format = "int64";
            }
            else if (type == typeof(ushort))
            {
                name = "integer";
                format = "int16";
            }
            else if (type == typeof(byte))
            {
                name = "integer";
                format = "int8";
            }
            else if (type == typeof(decimal))
            {
                name = "number";
                format = "decimal";
            }
            else if (type == typeof(float))
            {
                name = "number";
                format = "float";
            }
            else if (type == typeof(double))
            {
                name = "number";
                format = "double";
            }
            else if (type == typeof(DateTime) || type == typeof(DateTimeOffset))
            {
                name = "string";
                format = "date-time";
            }
            else if (type == typeof(Guid))
            {
                name = "string";
                format = "guid";
            }
        }
    }
}
