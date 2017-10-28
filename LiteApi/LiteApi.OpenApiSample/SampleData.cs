using LiteApi.OpenApiSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteApi.OpenApiSample
{
    public static class SampleData
    {
        public static IReadOnlyCollection<Author> Authors { get; }

        public static IReadOnlyCollection<Book> Books { get; }
        
        public static IReadOnlyCollection<Order> Orders { get; }

        static SampleData()
        {
            Authors = new Author[]
            {
                new Author
                {
                    BirthYear = 1928,
                    Id = Guid.NewGuid(),
                    Name = "Philip K. Dick"
                },
                new Author
                {
                    Id = Guid.NewGuid(),
                    Name = "Fyodor Dostoyevsky",
                    BirthYear = 1821
                }
            };

            Books = new Book[]
            {
                new Book
                {
                    ISBN = "0143058142",
                    Author = Authors.First(x => x.Name == "Fyodor Dostoyevsky"),
                    Genres = new Genre[] { Genre.Fiction },
                    Name = "Crime and Punishment",
                    PublishYear = 1866
                },
                new Book
                {
                    ISBN = "8498000831",
                    PublishYear = 1969,
                    Author = Authors.First(x => x.Name == "Philip K. Dick"),
                    Genres = new Genre[] { Genre.ScienceFiction },
                    Name = "Ubik"
                }
            };

            Orders = new Order[]
            {
                new Order
                {
                    Books = new Book[] { Books.First() },
                    Id = Guid.NewGuid(),
                    Orderer = new Orderer
                    {
                        Address = new Address
                        {
                            City = "Diependaalselaan 47",
                            Street1 = "Hilversum",
                            Country = "The Netherlands",
                            Region = "Noord-Holland",
                            PostalCode = "1216 GE"
                        },
                        Name = "Belgin J van Bruggen",
                        Email = "example@example.com",
                        PhoneNumber = "+31 6 73398820"
                    }
                }
            };
        }
    }
}
