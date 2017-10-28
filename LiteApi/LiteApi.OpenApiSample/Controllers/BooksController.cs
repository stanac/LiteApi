using LiteApi.Attributes;
using LiteApi.OpenApi.Attributes;
using LiteApi.OpenApiSample.DataAccess;
using LiteApi.OpenApiSample.Models;
using System.Collections.Generic;
using System.Linq;

namespace LiteApi.OpenApiSample.Controllers
{
    [Restful]
    public class BooksController : LiteController
    {
        private readonly IBooksDataAccess _data;

        public BooksController(IBooksDataAccess data)
        {
            _data = data;
        }

        [HttpPost, OpenApiOperation("AddBook", 201)]
        public Book Add(Book model)
        {
            SetResponseStatusCode(201);
            return _data.Add(model);
        }

        [HttpDelete, ActionRoute("/{isbn}"), OpenApiOperation("DeleteBook", 200, 404)]
        public bool Delete(string isbn)
        {
            bool deleted = _data.Delete(isbn);
            if (!deleted)
            {
                SetResponseStatusCode(404);
            }
            return deleted;
        }

        [HttpGet, OpenApiOperation("GetAllBooks", 200)]
        public IEnumerable<Book> Get()
        {
            return _data.GetAll();
        }

        [HttpGet, ActionRoute("/{isbn}"), OpenApiOperation("GetBookById", 200, 404)]
        public Book Get(string isbn)
        {
            var model = _data.Get(isbn);
            if (model == null) SetResponseStatusCode(404);
            return model;
        }

        [HttpGet, ActionRoute("/genres/{genre}"), OpenApiOperation("GetBooksByGenres", 200)]
        public IEnumerable<Book> GetByGenre(string genre)
        {
            genre = (genre ?? "").ToLower();
            return _data.GetAll().Where(x => x.Genres.Any(g => g.ToString().ToLower() == genre));
        }
        
        [HttpPut, ActionRoute("/{isbn}"), OpenApiOperation("UpdateBook", 200, 404)]
        public ApiCallResult<Book> Update(string isbn, Book model)
        {
            bool exists = _data.Get(isbn) != null;
            if (!exists)
            {
                SetResponseStatusCode(404);
                return new ApiCallResult<Book>
                {
                    Success = false,
                    Message = "Not found"
                };
            }
            var retModel = _data.Update(isbn, model);
            return new ApiCallResult<Book>
            {
                Data = retModel,
                Success = true
            };
        }
    }
}
