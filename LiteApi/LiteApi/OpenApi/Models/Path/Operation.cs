using Newtonsoft.Json;
using System.Collections.Generic;

namespace LiteApi.OpenApi.Models.Path
{
    public class Operation
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Tag[] Tags { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Summary { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string OperationId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Consumes { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] Produces { get; set; }
        public Dictionary<int, Response> Responses { get; set; } = new Dictionary<int, Response>();
        public Parameter[] Parameters { get; set; }
    }
}
