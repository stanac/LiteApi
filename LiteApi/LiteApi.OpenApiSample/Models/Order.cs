using System;
using System.ComponentModel.DataAnnotations;

namespace LiteApi.OpenApiSample.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        [Required]
        public Orderer Orderer { get; set; }
        [Required, MinLength(1)]
        public Book[] Books { get; set; }
    }
}
