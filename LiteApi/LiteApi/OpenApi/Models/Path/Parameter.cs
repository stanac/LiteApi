using LiteApi.OpenApi.Models.Definition;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteApi.OpenApi.Models.Path
{
    public class Parameter
    {
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the in.
        /// </summary>
        /// <value>
        /// The in. Valid values: path, query, header, body
        /// </value>
        public string In { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        public bool Required { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ScehmaObject Schema { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Format { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Items { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Enum { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CollectionFormat { get; set; }
    }
}
