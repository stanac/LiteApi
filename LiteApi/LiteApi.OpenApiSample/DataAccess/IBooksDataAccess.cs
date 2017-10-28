using LiteApi.OpenApiSample.Models;
using System.Collections.Generic;

namespace LiteApi.OpenApiSample.DataAccess
{
    public interface IBooksDataAccess
    {
        IEnumerable<Book> GetAll();
        Book Get(string isbn);
        Book Add(Book model);
        bool Delete(string isbn);
        Book Update(string isbn, Book model);
    }
}