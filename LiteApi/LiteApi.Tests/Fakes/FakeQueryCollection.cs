using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using System.Collections;

namespace LiteApi.Tests.Fakes
{
    public class FakeQueryCollection : IQueryCollection
    {
        private Dictionary<string, StringValues> _collection = new Dictionary<string, StringValues>();

        public StringValues this[string key] => _collection[key];

        public int Count => _collection.Count;

        public ICollection<string> Keys => _collection.Keys;

        public bool ContainsKey(string key) => _collection.ContainsKey(key);

        public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator() => _collection.GetEnumerator();

        public bool TryGetValue(string key, out StringValues value) => _collection.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();

        public void Add(string key, string value)
        {
            _collection[key] = new StringValues(value);
        }

        public void Clear()
        {
            _collection.Clear();
        }
    }
}
