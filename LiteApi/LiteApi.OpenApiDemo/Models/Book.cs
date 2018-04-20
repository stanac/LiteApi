using System;

namespace LiteApi.OpenApiDemo.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? PublishYear { get; set; }
        public Genre[] Genres { get; set; }
        public Guid AuthorId { get; set; }
        public string Synopsis { get; set; }
    }
}
