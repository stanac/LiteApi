using Newtonsoft.Json;

namespace LiteApi.OpenApi.Models.Path
{
    public class Path
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Operation Get { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Operation Post { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Operation Put { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Operation Delete { get; set; }
    }
}
