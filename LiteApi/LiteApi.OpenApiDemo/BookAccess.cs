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
            Genres.AddRange(sampleData.Books
                .SelectMany(x => x.Generes)
                .GroupBy(x => x.Id)
                .Select(x => x.First()));
            Books.AddRange(sampleData.Books);
            Authors.AddRange(sampleData.Authors);
        }

        public List<Author> Authors { get; } = new List<Author>();
        public List<Book> Books { get; } = new List<Book>();
        public List<Genre> Genres { get; } = new List<Genre>();
        
    }
}
