using System.ComponentModel.DataAnnotations;

namespace LiteApi.OpenApiSample.Models
{
    public class Address
    {
        [Required]
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Region { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
