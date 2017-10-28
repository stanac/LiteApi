using LiteApi.OpenApi.Models.Definition;
using Newtonsoft.Json;

namespace LiteApi.OpenApi.Models.Path
{
    public class Response
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        public ScehmaObject Schema { get; set; }
    }
}
