using System.ComponentModel.DataAnnotations;

namespace LiteApi.OpenApiSample.Models
{
    public class Book
    {
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, Range(1000, 2100)]
        public int PublishYear { get; set; }
        [Required, MinLength(1)]
        public Genre[] Genres { get; set; }
        [Required]
        public Author Author { get; set; }
    }
}
