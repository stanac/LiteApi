using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using System.Collections;

namespace LiteApi.Tests.Fakes
{
    public class FakeFormCollection : IFormCollection
    {
        public Dictionary<string, StringValues> FormItems { get; } = new Dictionary<string, StringValues>();

        public StringValues this[string key] => FormItems[key];

        public int Count => FormItems.Count;

        public IFormFileCollection Files { get; } = new FakeFormFileCollection();

        public ICollection<string> Keys => FormItems.Keys;

        public bool ContainsKey(string key) => FormItems.ContainsKey(key);

        public IEnumerator<KeyValuePair<string, StringValues>> GetEnumerator() => FormItems.GetEnumerator();

        public bool TryGetValue(string key, out StringValues value) => FormItems.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
