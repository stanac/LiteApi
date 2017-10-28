using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LiteApi.OpenApi.Models.Definition
{
    public class ScehmaObject
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Format { get; set; }
        [JsonProperty(PropertyName = "$ref", NullValueHandling = NullValueHandling.Ignore)]
        public string Reference { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ScehmaObject Items { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Enum { get; set; }
        
        [JsonIgnore]
        public Type OriginalType { get; set; }
        [JsonIgnore]
        public string DesieredTypeId { get; set; }
        [JsonIgnore]
        public string ActualTypeId { get; set; }

        public static ScehmaObject FromType(Type type)
        {
            ScehmaObject retObj = new ScehmaObject
            {
                OriginalType = type
            };

            Type nullableArg;
            if (type.IsNullable(out nullableArg))
            {
                type = nullableArg;
            }

            if (PrimitiveTypeConverter.IsTypePrimitive(type))
            {
                string typeName, format;
                PrimitiveTypeConverter.GetTypeNameAndFormat(type, out typeName, out format);
                retObj.Type = typeName;
                retObj.Format = format;
            }
            else if (type.GetTypeInfo().IsGenericType && typeof(IEnumerable<>).IsAssignableFrom(type))
            {
                Type arg = type.GetGenericArguments().First();
                Type temp;
                if (arg.IsNullable(out temp))
                {
                    arg = temp;
                }
                retObj.Type = "array";
                retObj.Items = FromType(arg);
            }
            else if (type.GetTypeInfo().IsEnum)
            {
                retObj.Type = "string";
                retObj.Enum = System.Enum.GetNames(type);
            }
            retObj.DesieredTypeId = type.Name;

            // complex type
            retObj.Type = "object";


            return retObj;
        }
    }
}