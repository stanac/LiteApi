using System;

namespace LiteApi.Services
{
    public interface IObjectBuilder
    {
        object BuildObject(Type objectType);
        T BuildObject<T>() where T : class;
    }
}