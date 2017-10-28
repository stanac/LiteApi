using System;
using System.ComponentModel.DataAnnotations;

namespace LiteApi.OpenApiSample.Models
{
    public class Author
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int? BirthYear { get; set; }
    }
}
