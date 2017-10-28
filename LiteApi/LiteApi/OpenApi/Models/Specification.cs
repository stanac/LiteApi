using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace LiteApi.OpenApi.Models
{
    class Specification
    {
        public string Swagger { get; } = "2.0";
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Host { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string BasePath { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Schemes { get; set; } = { "http" };
        public Info.Info Info { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ExternalDocumentation ExternalDocs { get; set; }
        public string[] Consumes { get; set; } = { "application/json" };
        public string[] Produces { get; set; } = { "application/json" };
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Tag[] Tags { get; set; }
        /// <summary>
        /// Gets or sets the paths.
        /// </summary>
        /// <value>
        /// The paths.
        /// </value>
        /// <example>
        /// Paths.
        /// </example>
        public Dictionary<string, object> Paths { get; set; } = new Dictionary<string, object>();
        public Dictionary<string, Definition.Definition> Definitions { get; set; } = new Dictionary<string, Definition.Definition>();

    }
}
