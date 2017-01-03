using LiteApi.Contracts.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Tests.Fakes
{
    public class FakeJsonSerializer : IJsonSerializer
    {
        public object Deserialize(string json, Type objectType)
        {
            throw new NotImplementedException();
        }

        public T Deserialize<T>(string json)
        {
            throw new NotImplementedException();
        }

        public string Serialize(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
