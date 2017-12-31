using LiteApi.Attributes;
using LiteApi.OpenApiDemo.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiteApi.OpenApiDemo.Api
{
    [Restful]
    public class GenresController: LiteController
    {
        private readonly BookAccess _bookAccess;

        public GenresController(BookAccess bookAccess)
        {
            _bookAccess = bookAccess ?? throw new ArgumentNullException(nameof(bookAccess));
        }

        public List<Genre> Get()
        {
            return _bookAccess.Genres;
        }

        [ActionRoute("/{id}")]
        public Genre GetById(Guid id)
        {
            return _bookAccess.Genres.FirstOrDefault(x => x.Id == id);
        }

        [HttpPost]
        public ApiResponse<Genre> Create([FromBody, Required] string genreName) // in case of string [Required] needs to be set explicitly
        {
            if (string.IsNullOrWhiteSpace(genreName))
            {
                SetResponseStatusCode(StatusCodes.Status400BadRequest);
                return new ApiResponse<Genre>
                {
                    Success = false,
                    Message = "genreName is not provided"
                };
            }
            var existingGenre = _bookAccess.Genres.FirstOrDefault(x => x.Name.ToLower() == genreName.ToLower());
            if (existingGenre != null)
            {
                SetResponseStatusCode(StatusCodes.Status400BadRequest);
                return new ApiResponse<Genre>
                {
                    Success = false,
                    Data = existingGenre,
                    Message = "Genre already exists"
                };
            }

            var newGenre = new Genre
            {
                Id = Guid.NewGuid(),
                Name = genreName
            };
            _bookAccess.Genres.Add(newGenre);

            SetResponseStatusCode(StatusCodes.Status201Created);
            AddResponseHeader("location", "/api/genres/" + newGenre.Id);
            return new ApiResponse<Genre>
            {
                Data = newGenre,
                Message = "Created",
                Success = true
            };
        }
    }
}
