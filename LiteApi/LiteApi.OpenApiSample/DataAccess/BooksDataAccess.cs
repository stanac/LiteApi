using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteApi.OpenApiSample.Models;

namespace LiteApi.OpenApiSample.DataAccess
{
    public class BooksDataAccess : IBooksDataAccess
    {
        private static List<Book> _books;

        static BooksDataAccess()
        {
            _books = new List<Book>();
            _books.AddRange(SampleData.Books);
        }

        public Book Add(Book model)
        {
            _books.Add(model);
            return model;
        }

        public bool Delete(string isbn)
        {
            var toDelete = _books.FirstOrDefault(x => x.ISBN == isbn);
            if (toDelete != null)
            {
                _books.Remove(toDelete);
                return true;
            }
            return false;
        }

        public Book Get(string isbn)
        {
            return _books.FirstOrDefault(x => x.ISBN == isbn);
        }

        public IEnumerable<Book> GetAll()
        {
            return _books;
        }

        public Book Update(string isbn, Book model)
        {
            model.ISBN = isbn;
            var toUpdate = _books.FirstOrDefault(x => x.ISBN == isbn);
            if (toUpdate != null)
            {
                toUpdate.PublishYear = model.PublishYear;
                toUpdate.Name = model.Name;
                toUpdate.Genres = model.Genres;
                toUpdate.Author = model.Author;
                return toUpdate;
            }
            return null;
        }
    }
}
