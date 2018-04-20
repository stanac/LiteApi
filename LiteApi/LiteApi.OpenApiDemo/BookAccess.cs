using LiteApi.OpenApiDemo.Models;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi.OpenApiDemo
{
    public class BookAccess
    {
        public BookAccess()
        {
            SampleDataReader.SampleData sampleData = new SampleDataReader().ReadSampleData();
            var allGeneres = sampleData.Books.SelectMany(x => x.Genres).ToArray();
            var groups = allGeneres.GroupBy(x => x.Id).ToArray();
            var distinct = groups.Select(x => x.First()).ToArray();
            Genres.AddRange(distinct);
            Books.AddRange(sampleData.Books);
            Authors.AddRange(sampleData.Authors);
        }

        public List<Author> Authors { get; } = new List<Author>();
        public List<Book> Books { get; } = new List<Book>();
        public List<Genre> Genres { get; } = new List<Genre>();
        
    }
}
