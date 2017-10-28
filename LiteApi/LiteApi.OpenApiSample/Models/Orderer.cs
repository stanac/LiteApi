using System.ComponentModel.DataAnnotations;

namespace LiteApi.OpenApiSample.Models
{
    public class Orderer
    {
        [Required, MinLength(5)]
        public string Name { get; set; }
        [Required]
        public Address Address { get; set; }
        public string PhoneNumber { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
