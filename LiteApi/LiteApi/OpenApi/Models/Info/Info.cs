using Newtonsoft.Json;

namespace LiteApi.OpenApi.Models.Info
{
    public class Info
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }
        public string Title { get; set; } = "";
        public string Version { get; set; } = "1.0";
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TermsOfService { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Contact Contact { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public License License { get; set; }
    }
}
