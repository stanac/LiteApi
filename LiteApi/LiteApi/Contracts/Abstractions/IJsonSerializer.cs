using System;

namespace LiteApi.Contracts.Abstractions
{
    /// <summary>
    /// Contract used for serialization and deserialization of JSON. It can be replaced to alter the behavior or implementation.
    /// Default implementation is using <see cref="Newtonsoft.Json.JsonSerializer"/> with camelCase settings.
    /// </summary>
    public interface IJsonSerializer
    {
        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>string, JSON</returns>
        string Serialize(object obj);

        /// <summary>
        /// Deserializes the specified JSON.
        /// </summary>
        /// <typeparam name="T">Type or object to return</typeparam>
        /// <param name="json">The JSON to deserialize.</param>
        /// <returns>Deserialized object with type of T</returns>
        T Deserialize<T>(string json);

        /// <summary>
        /// Deserializes the specified JSON.
        /// </summary>
        /// <param name="json">The JSON to deserialize.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>Deserialized object</returns>
        object Deserialize(string json, Type objectType);
    }
}
