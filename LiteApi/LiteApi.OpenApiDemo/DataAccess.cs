using LiteApi.OpenApiDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteApi.OpenApiDemo
{
    public class DataAccess
    {
        public List<Author> Authors { get; } = new List<Author>();
        public List<Book> Books { get; } = new List<Book>();
        public List<Genre> Genres { get; } = new List<Genre>();


    }
}
