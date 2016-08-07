using LiteApi.Contracts.Abstractions;
using Newtonsoft.Json;

namespace LiteApi.Services
{
    public class JsonSerializer : IJsonSerializer
    {
        public T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json);

        public string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj);
    }
}
