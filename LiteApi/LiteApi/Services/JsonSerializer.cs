using System;
using LiteApi.Contracts.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LiteApi.Services
{
    /// <summary>
    /// Default implementation of <see cref="IJsonSerializer"/>
    /// </summary>
    /// <seealso cref="LiteApi.Contracts.Abstractions.IJsonSerializer" />
    public class JsonSerializer : IJsonSerializer
    {
        private static JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            PreserveReferencesHandling = PreserveReferencesHandling.None,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        /// <summary>
        /// Deserializes the specified JSON.
        /// </summary>
        /// <param name="json">The JSON to deserialize.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// Deserialized object
        /// </returns>
        public object Deserialize(string json, Type objectType) => JsonConvert.DeserializeObject(json, objectType, _settings);

        /// <summary>
        /// Deserializes given JSON
        /// </summary>
        /// <typeparam name="T">Type of object to deserialize to</typeparam>
        /// <param name="json">JSON to deserialize</param>
        /// <returns>Deserialized object</returns>
        public T Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, _settings);

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>string, JSON</returns>
        public string Serialize(object obj) => JsonConvert.SerializeObject(obj, _settings);
    }
}
