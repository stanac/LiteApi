using System;

namespace LiteApi.Contracts.Abstractions
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string json);
        object Deserialize(string json, Type objectType);
    }
}
