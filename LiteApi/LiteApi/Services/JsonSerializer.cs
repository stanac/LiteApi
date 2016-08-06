using LiteApi.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteApi.Services
{
    public class JsonSerializer : IJsonSerializer
    {
        public T Deserialize<T>(string json)
        {
            throw new NotImplementedException();
        }

        public string Serialize<T>(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
