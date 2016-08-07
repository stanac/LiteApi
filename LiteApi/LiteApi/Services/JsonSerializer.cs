using System;
using LiteApi.Contracts.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LiteApi.Services
{
    public class JsonSerializer : IJsonSerializer
    {
        private static JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public object Deserialize(string json, Type objectType) => JsonConvert.DeserializeObject(json, objectType, _settings);

        public T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, _settings);

        public string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj, _settings);
    }
}
