using System;

namespace LiteApi.Contracts.Abstractions
{
    public interface IObjectBuilder
    {
        object BuildObject(Type objectType);
        T BuildObject<T>() where T : class;
    }
}