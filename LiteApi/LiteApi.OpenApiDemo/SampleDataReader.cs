using LiteApi.OpenApiDemo.Models;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace LiteApi.OpenApiDemo
{
    public class SampleDataReader
    {
        public class SampleData
        {
            public Author[] Authors { get; set; }
            public Book[] Books { get; set; }
        }

        public SampleData ReadSampleData()
        {
            string resourceName = typeof(SampleDataReader).Assembly.GetManifestResourceNames().First(x => x.Contains("sampleData.json"));
            using (Stream s = typeof(SampleDataReader).Assembly.GetManifestResourceStream(resourceName))
            using (TextReader reader = new StreamReader(s))
            {
                string json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<SampleData>(json);
            }
        }
    }
}
