using LiteApi.OpenApiSample.DataAccess;
using System;
using System.Collections.Generic;
using LiteApi.OpenApiSample.Models;
using Microsoft.AspNetCore.Http;
using LiteApi.OpenApi.Attributes;
using LiteApi.Attributes;

namespace LiteApi.OpenApiSample.Controllers
{
    [Restful]
    public class AuthorsController: LiteController
    {
        private readonly IAuthorsDataAccess _data;

        public AuthorsController(IAuthorsDataAccess data)
        {
            _data = data;
        }

        [HttpPost, OpenApiOperation("AddAuthor", 201)]
        public Author Add(Author model)
        {
            SetResponseStatusCode(201);
            return _data.Add(model);
        }

        [HttpDelete, ActionRoute("/{id}"), OpenApiOperation("DeleteAuthor", 200, 404)]
        public bool Delete(Guid id)
        {
            bool deleted = _data.Delete(id);
            if (!deleted)
            {
                SetResponseStatusCode(404);
                return false;
            }
            return true;
        }

        [HttpGet, ActionRoute("/{id}"), OpenApiOperation("GetAuthorById", 200, 404)]
        public Author Get(Guid id)
        {
            var author = _data.Get(id);
            if (author == null)
            {
                SetResponseStatusCode(404);
                return null;
            }
            return author;
        }

        [HttpGet, OpenApiOperation("GetAllAuthors", 200)]
        public IEnumerable<Author> GetAll()
            => _data.GetAll();

        [HttpPut, ActionRoute("/{id}"), OpenApiOperation("UpdateAuthor", 200, 404)]
        public ApiCallResult<Author> Update(Guid id, Author model)
        {
            var author = _data.Update(id, model);
            if (author == null)
            {
                SetResponseStatusCode(StatusCodes.Status404NotFound);
                return new ApiCallResult<Author>
                {
                    Message = "Not found"
                };
            }

            return new ApiCallResult<Author>
            {
                Data = author,
                Success = true,
                Message = "Updated"
            };
        }
    }
}
