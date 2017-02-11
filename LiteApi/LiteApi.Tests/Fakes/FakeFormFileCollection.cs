using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace LiteApi.Tests.Fakes
{
    public class FakeFormFileCollection : IFormFileCollection
    {
        public Dictionary<string, FakeFormFile> Data { get; } = new Dictionary<string, FakeFormFile>();

        public IFormFile this[int index] => Data.Select(x => x.Value).ElementAt(index);

        public IFormFile this[string name] => Data[name];

        public int Count => Data.Count;

        public IEnumerator<IFormFile> GetEnumerator() => Data.Select(x => x.Value).GetEnumerator();

        public IFormFile GetFile(string name) => this[name];

        public IReadOnlyList<IFormFile> GetFiles(string name)
        {
            List<IFormFile> l = new List<IFormFile>();
            l.Add(this[name]);
            IReadOnlyList<IFormFile> readonlyList = l;
            return l;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
