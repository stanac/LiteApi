using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LiteApi.OpenApi.Models.Definition
{
    public class Definition
    {
        public string Type { get; set; } = "object";
        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>
        /// The properties.
        /// </value>
        public Dictionary<string, ScehmaObject> Properties { get; set; } = new Dictionary<string, ScehmaObject>();

        [JsonIgnore]
        public string TypeFullName { get; set; }

        [JsonIgnore]
        public Type OriginalType { get; set; }

        [JsonIgnore]
        public string DesiredTypeId { get; set; }

        [JsonIgnore]
        public string ActualTypeId { get; set; }

        public static Definition FromType(Type type)
        {
            var definition = new Definition();
            definition.OriginalType = type;

            return definition;
        }

    }
}
