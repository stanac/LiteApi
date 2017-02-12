using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Primitives;
using System.Collections;

namespace LiteApi.Tests.Fakes
{
    public class FakeHeaderDictionary : IHeaderDictionary
    {
        private readonly IDictionary<string, StringValues> _data = new Dictionary<string, StringValues>();

        public StringValues this[string key]
        {
            get
            {
                return _data[key];
            }
            set
            {
                _data[key] = value;
            }
        }

        public int Count => _data.Count;

        public bool IsReadOnly => false;

        public ICollection<string> Keys => _data.Select(x => x.Key).ToList();

        public ICollection<StringValues> Values => _data.Select(x => x.Value).ToList();

        public void Add(KeyValuePair<string, StringValues> item) => _data.Add(item.Key, item.Value);

        public void Add(string key, StringValues value) => _data.Add(key, value);

        public void Clear() => _data.Clear();

        public bool Contains(KeyValuePair<string, StringValues> item) => _data.Contains(item);

        public bool ContainsKey(string key) => _data.ContainsKey(key);

        public void CopyTo(KeyValuePair<string, StringValues>[] array, int arrayIndex) => _data.CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator() => _data.GetEnumerator();

        public bool Remove(KeyValuePair<string, StringValues> item) => _data.Remove(item);

        public bool Remove(string key) => _data.Remove(key);

        public bool TryGetValue(string key, out StringValues value) => _data.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
